export const assetTypes = {
    "Retail Tenant Property": ["Single Tenant", "Multiple Tenant with Anchor", "Multiple Tenant without Anchor", "Publically Traded Single Tenant", "Publically Traded Anchor Tenant", "Restaurant Franchise", "Grocery Franchise", "Retail Franchise", "Factory Outlet Center", "Government Tenant", "Regional Mall"],
    "Office Tenant Property": ["Single Tenant", "Publically Traded Single Tenant", "Multiple Tenant", "Publically Traded Major Tenant", "High Rise", "Mid Rise", "Garden Style", "Office Condominium Development", "Government Tenant"],
    "Multi-Family": ["Student Housing Development", "Retirement Housing Development", "Garden Style", "High Rise", "Mid Rise", "Section 8 Housing - Government Assisted", "LIHTC", "Property w/ 1+ Community Pool/Spa"],
    "Industrial Tenant Property": ["Single Tenant", "Multiple Tenant", "Publically Traded Single Tenant", "Publically Traded Multiple Tenant", "Warehouse - Single Tenant", "Warehouse - Multiple Tenant", "Light Manufacturing", "Heavy Manufacturing", "Office - Warehouse"],
    "MHP": ["MHP Properties with or without RV Facilities", "MHP Properties without RV Facilities", "MHP Properties with Only Affixed Units", "MHP Properties with Affixed or Above Ground Units", "MHP Properties with HOAs", "RV Parks and Campsites without MHP Spaces"],
    "Fuel Service Retail Property": ["Convenience Store Franchise - No Fuel", "Convenience Store Franchise - With Fuel", "Convenience Store - No Fuel", "Convenience Store - With Fuel", "Multi-Purpose Trucking Service Facility", "Auto Care Facility - With Fuel", "Auto Care Facility - No Fuel", "Franchised Auto Care Facility"],
    "Medical Tenant Property": ["Other", "Resort/Hotel/Motel Property", "Franchised Tenant", "Publically Traded Franchised Tenant", "Publically Traded Major Tenant", "Single Tenant", "Multiple Tenant", "Research Facility", "Rehabilitation Facility", "Assisted Living Facility"],
    "Mixed Use Commercial Property": [],
    "Fractured Condominium Portfolios": [],
    "Mini-Storage Property": [],
    "Parking Garage Property": [],
    "Secured Privated Notes": []
}

export const assetyTypesAndIDs = [
    { name: "Retail Tenant Property", id: 1, },
    { name: "Office Tenant Property", id: 2 },
    { name: "Multi-Family", id: 3 },
    { name: "Industrial Tenant Property", id: 4 },
    { name: "MHP", id: 5 },
    { name: "Fuel Service Retail Property", id: 6 },
    { name: "Medical Tenant Property", id: 7 },
    { name: "Mixed Use Commercial Property", id: 8 },
    { name: "Fractured Condominium Portfolios", id: 13 },
    { name: "Mini-Storage Property", id: 14 },
    { name: "Parking Garage Property", id: 15 },
    { name: "Secured Privated Notes", id: 16 }
]

export const pricingTypes = {
    "CMV": [],
    "LP": []
}

export const gradeTypes = {
    "A+": [],
    "A": [],
    "A-": [],
    "B+": [],
    "B": [],
    "B-": [],
    "C+": [],
    "C": [],
    "C-": [],
    "D+": [],
    "D": [],
    "D-": [],
    "F": []
}

export const operatingTypes = {
    "FDIC": [],
    "In Default - FDIC Control": [],
    "Pending Forclosure": [],
    "Private - In Default": [],
    "Private - Not In Default": [],
    "REO": []
}

export const binaryTypes = {
    "Yes": [],
    "No": []
}

export const rentableTypes = {
    "Less Than or Equal to 99": [],
    "100-199": [],
    "200-399": [],
    "400 or More": []
}

export const amortizationTypes = {
    "Interest Only": [],
    "10 Year Amortization": [],
    "15 Year Amortization": [],
    "20 Year Amortization": [],
    "25 Year Amortization": [],
    "30 Year Amortization": []
}

export const maturityScheduleTypes = {
    "Fully Amortized - No Maturity Date": [],
    "3 Years from Date of Search": [],
    "3-6 Years from Date of Search": [],
    "6-10 Years from Date of Search": [],
    "10 Years from Date of Search": []
}

export const positionTypes = {
    "1st Position, Institutional": [],
    "2nd Position, Institutional": [],
    "2nd Position, Private Party Beneficiary": [],
    "Wrap Around, Encompassing 1 Mortgage": [],
    "Wrap Around, Encompassing 2 Mortgages": []
}

export const instrumentTypes = {
    "Deed of Trust": [],
    "Mortgage": [],
    "Land Contract": [],
    "Contract for Deed": [],
    "All Inclusive Wrap Around": []
}

export const balanceNoteTypes = {
    "90% - 100%+": [],
    "80% - 89.99%": [],
    "70% - 79.99%": [],
    "60% - 69.99%": [],
    "50% - 59.99%": []
}

export const generalAdvanced = [
    {name: "Location", value: "", width: "300px", placeholder: "Las Vegas, NV", type: "text", inputtype: "field"},
    {name: "Min. Price", value: "", width: "132px", placeholder: "$15,000,000", type: "money", inputtype: "field"},
    {name: "Max Price", value: "", width: "132px", placeholder: "$15,000,000", type: "money", inputtype: "field"},
    {name: "Min Cap %", value: "", width: "86px", placeholder: "5.5%", type: "percent", inputtype: "field"},
    {name: "Min Year Built", value: "", width: "125px", placeholder: "1990", type: "number", inputtype: "field"},
    {name: "Asset Type", value: [], width: "300px", placeholder: "Industrial Tenant Property", type: "text", inputtype: "dropdown", options: assetTypes, dropplaceholder: "Select Asset Type"},
    {name: "Pricing Type", value: [], width: "125px", placeholder: "Type", type: "text", inputtype: "dropdown", options: pricingTypes, dropplaceholder: "Type"},
    {name: "Saved Search", value: "", width: "273px", placeholder: "Enter Search Name", type: "text", inputtype: "field"}
]