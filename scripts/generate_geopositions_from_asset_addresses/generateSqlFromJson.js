const fs = require('fs-extra');

let sqlScript = '-- Update Latitude and Longitudes on the Assets\n';

init();
// example entry: {"assetId":"22432D80-22AD-402C-9F82-001C06EDDCB3","assetNumber":7912,"lat":43.1412223,"lng":-76.12742}

function init() {
  fs.readJson('epifund_asset_geopositions.json', (err, data) => {
    if (err) {
      console.err(err);
      process.exit(0);
    } else {
      for (let i = 0; i < data.length; i++) {
        sqlScript += `-- #${data[i].assetNumber}\n`;
        sqlScript += `UPDATE [uscreonline].[dbo].Assets SET Latitude = ${data[i].lat}, Longitude = ${data[i].lng} WHERE AssetId = '${data[i].assetId}'\n`
      }
      fs.writeFile('epifund_update_asset_geopositions.sql', sqlScript, err0 => {
        if (err0) throw err0;
        else {
          console.log(`SQL generated with ${data.length} rows`)
        }
      })
    }
  })
}
