// WE CAN GET SQL INJECTED USING THIS BLINDLY
const fs = require('fs'),
  uuidv4 = require('uuid/v4')

let sqlScript = '',sql,count=0;
sqlScript += `-- Create Initial Unknown Operating Company\n`
sqlScript += `INSERT INTO [uscreonline].[dbo].[OperatingCompanies] VALUES ('00000000-0000-0000-0000-000000000000','Unknown','','','','','','','','','','','','',1)\n\n`

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
function generate(conn) {
  conn.connect().then(() => {
    //simple query 'select TOP (2) from Assets' 'select 1 as number'
    conn.request().query('select [AssetId], [AssetNumber], [Owner], [ContactPhoneNumber] ,[CorporateOwnershipAddress] ,[CorporateOwnershipAddress2] ,[CorporateOwnershipCity] ,[CorporateOwnershipState] ,[CorporateOwnershipZip] ,[CorporateOwnershipOfficer] from Assets', (err, result) => {
      console.log(`${result.recordset.length} assets`)
      if (result.recordset.length > 0) {
        for (let i = 0; i < result.recordset.length; i++) {
          if (result.recordset[i].Owner && typeof result.recordset[i].Owner === 'string' && result.recordset[i].Owner.trim().length > 0) {
            count++
            let hcId = uuidv4(),firstName='',lastName='';
            // attempt to parse names
            if (result.recordset[i].CorporateOwnershipOfficer && typeof result.recordset[i].CorporateOwnershipOfficer === 'string' && result.recordset[i].CorporateOwnershipOfficer.trim().length > 0) {
              // lets only parse names if we encounter one whitespace in the string(probably indicating it was a human name) for now
              let splitRes = result.recordset[i].CorporateOwnershipOfficer.trim().split(' ')
              if (splitRes.length === 2) {
                //console.log(`found possible name: ${result.recordset[i].AssetNumber} - ${result.recordset[i].CorporateOwnershipOfficer}`)
                firstName = splitRes[0]
                lastName = splitRes[1]
              } else {
                //console.log(`setting to first name: ${result.recordset[i].AssetNumber} - ${result.recordset[i].CorporateOwnershipOfficer}`)
                firstName = result.recordset[i].CorporateOwnershipOfficer.trim()
              }
            }
            //sqlScript = `INSERT INTO [uscreonline].[dbo].[HoldingCompanies] VALUES ('${uuidv4}','00000000-0000-0000-0000-000000000000','${result.recordset[i].Owner}','fn','ln','em','addr1','addr2','city','state','z','country','w#','c#','f#',1)`
            sqlScript += `--${result.recordset[i].AssetNumber}\n`
            sqlScript += `INSERT INTO [uscreonline].[dbo].[HoldingCompanies] VALUES ('${hcId}','00000000-0000-0000-0000-000000000000','${parseVal(result.recordset[i].Owner.trim())}','${parseVal(firstName)}','${parseVal(lastName)}','','${parseVal(result.recordset[i].CorporateOwnershipAddress)}','${parseVal(result.recordset[i].CorporateOwnershipAddress2)}','${parseVal(result.recordset[i].CorporateOwnershipCity)}','${parseVal(result.recordset[i].CorporateOwnershipState)}','${parseVal(result.recordset[i].CorporateOwnershipZip)}','usa','${parseVal(result.recordset[i].ContactPhoneNumber)}','','',1)\n`
            sqlScript += `UPDATE [uscreonline].[dbo].Assets SET OwnerHoldingCompanyId = '${hcId}' WHERE AssetId = '${result.recordset[i].AssetId}'\n\n`
          }
        }
        fs.writeFile('epifund_updateThing.sql', sqlScript, err => {
          if (err) throw err
          console.log(`${count} holding companies`)
          process.exit(0);
        })
      }
    })
  })
}

function parseVal(val) {
  if (val && typeof val === 'string' && val.length > 0) {
    return val.replace(/'/g, "''");
  }
  return '';
}
