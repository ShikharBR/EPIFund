// WE CAN GET SQL INJECTED USING THIS BLINDLY
const fs = require('fs-extra'),
  uuidv4 = require('uuid/v4'),
  googleMapsClient = require('@google/maps').createClient({
    key: 'AIzaSyDqYSKwQkzv0nPfRWWRiVNOVoMIAQjSrok'
  });
// 2161, 2162 bad addresses. 2197 incomplete
//let assetQuery = 'select TOP 1 [AssetId], [AssetNumber], [PropertyAddress], [City] ,[State] ,[Zip] from [uscreonline].[dbo].Assets'; // get one asset for testing
let assetQuery = 'select [AssetId], [AssetNumber], [PropertyAddress], [City] ,[State] ,[Zip] from [uscreonline].[dbo].Assets ORDER BY [AssetNumber]';
let sqlScript = '',sql,count=0,errors=[],data=[],googleApiIntervalLimit=5000;
sqlScript += `-- Update Latitude and Longitudes on the Assets\n`

/************************************/
// ONLY SET THIS VALUE if you need to continue
let startBy = 0;
// Make sure the 'epifund_asset_geopositions.json' is at the same spot
/***********************************/

// call desired method here in order to... do the thing
local()

function remote() {
  sql = require('mssql')
  const conn = new sql.ConnectionPool({
    user: '',
    password: '',
    server: '',
    database: 'uscreonline'
  })
  generate(conn)
  sql.on('error', err => { throw err; })
}
function local() {
  sql = require("mssql/msnodesqlv8")
  const conn = new sql.ConnectionPool({
    driver: 'msnodesqlv8',
    server: 'localhost\\SQLExpress',
    database: 'uscreonline',
    options: {
      trustedConnection: true
    }
  })
  generate(conn)
  sql.on('error', err => { throw err; })
}
function getData(cb) {
  fs.readJson('epifund_asset_geopositions.json', (err, oldPositions) => {
    if (err) {
      // assume it doesnt exist, move on
      console.log('existing data does not exist')
    } else {
      data = oldPositions;
      console.log('data retrieved and set')
    }
    cb();
  })
}
function generate(conn) {
  conn.connect().then(() => {
    conn.request().query(assetQuery, (err, result) => {
      console.log(`${result.recordset.length} assets found`)
      if (result.recordset && result.recordset.length > 0) {
        // if startBy isnt set, dont even bother getting the existing data
        if (startBy > 0) { getData(function () { loopThroughResults(result.recordset); }) }
        else { loopThroughResults(result.recordset); }
      } else {
        console.log('record set is missing or no results, fix it')
        process.exit(0)
      }
    })
  })
}

function loopThroughResults(recordset) {
  processRow(0);
  function processRow(index) {
    if (index < recordset.length) {
      if (recordset[index].AssetNumber > startBy) {
        if (stringHasValue(recordset[index].PropertyAddress) && stringHasValue(recordset[index].City) && stringHasValue(recordset[index].State) && stringHasValue(recordset[index].Zip)) {
          // throttle how fast we talk to the google API because they like to limit people
          setTimeout(function() {
            googleMapsClient.geocode({
              address: `${recordset[index].PropertyAddress.trim()} ${recordset[index].City.trim()} ${recordset[index].State.trim()} ${recordset[index].Zip.trim()}`
            }, function(err, response) {
              if (!err) {
                // api is not returning an error, just an empty array when an address is bad
                if (response.json.results.length > 0) {
                  console.log(`${index+1} out of ${recordset.length}: Asset#: ${recordset[index].AssetNumber}, lat: ${response.json.results[0].geometry.location.lat}, lng: ${response.json.results[0].geometry.location.lng}`)
                  recordset[index].lat = response.json.results[0].geometry.location.lat;
                  recordset[index].lng = response.json.results[0].geometry.location.lng;
                  sqlScript += `UPDATE [uscreonline].[dbo].Assets SET Latitude = ${response.json.results[0].geometry.location.lat}, Longitude = ${response.json.results[0].geometry.location.lng} WHERE AssetId = '${recordset[index].AssetId}'\n\n`
                  appendDataToOneOfThoseArrays(recordset[index]);
                  // write data immediately
                  fs.writeJson('epifund_asset_geopositions.json', data, function (err1) {
                    if (err1) throw err1;
                    processRow(index + 1);
                  });
                } else {
                  console.log(`${index+1} out of ${recordset.length}: Asset#: ${recordset[index].AssetNumber}, address malformed(assumption)`)
                  appendDataToOneOfThoseArrays(recordset[index], 'address malformed');
                  processRow(index + 1);
                }
              } else {
                // no idea what an error actually looks like, hopefully this code works lol
                try {
                  console.log(`${index+1} out of ${recordset.length}: Asset#: ${recordset[index].AssetNumber}, Error: ${JSON.stringify(err)}`);
                  appendDataToOneOfThoseArrays(recordset[index], err);
                  processRow(index + 1);
                } catch(err) {
                  console.log(err)
                } finally {
                  processRow(index + 1);
                }
              }
            });
          }, googleApiIntervalLimit)
        } else {
          // record error
          console.log(`${recordset[index].AssetNumber} missing address data`)
          appendDataToOneOfThoseArrays(recordset[index], 'missing address data');
          processRow(index + 1);
        }
      } else {
        console.log(`Aleady processed ${recordset[index].AssetNumber}`)
        // slow things down for some reason?
        setTimeout(function() { processRow(index + 1); }, 200)
      }
    } else {
      // we processed all results, write out the files and what not
      doAllTheLastProcessThingsAndStuff();
    }
  }
}

function doAllTheLastProcessThingsAndStuff() {
  fs.writeFile('epifund_update_asset_geopositions.sql', sqlScript, err0 => {
    if (err0) throw err0
    fs.writeJson('epifund_asset_geopositions.json', data, function (err1) {
      if (err1) throw err1
      // errors is the error array declared at the top
      if (errors.length > 0) {
        fs.writeJson('epifund_asset_geoposition_errors.json', errors, function (err2) {
          if (err2) throw err2
          console.log('did all the things')
          // too lazy to close sql connection properly, just shut down the process immediately
          process.exit(0);
        })
      } else {
        console.log('complete');
        process.exit(0);
      }
    })
  })
}

function appendDataToOneOfThoseArrays(record, err) {
  if (err) {
    if (typeof err !== 'string') err = JSON.stringify(err);
    errors.push({ reason: err, assetId: record.AssetId, assetNumber: record.AssetNumber, address: record.PropertyAddress, city: record.City, state: record.State, zip: record.Zip })
  } else {
    data.push({ assetId: record.AssetId, assetNumber: record.AssetNumber, lat: record.lat, lng: record.lng })
  }
}

function stringHasValue(val) { return val && typeof val === 'string' && val.length > 0 && val.trim().length > 0; }
