const mongoose = require('mongoose');
const Schema = mongoose.Schema

const jobstatus = new Schema({
    customerid: String,
    driverid: String,
    customerFrom: String,
    customerTo: String,
    isJobdone: String,
    amountPayable: String,
    vehicleType: String,
    feedbackRatefordriver: String,
    feedbackrateforcustomer: String,
    feedbackforcustomer: String,
    feedbackfordriver: String,
    dateTime: String,
    Nonce: String,
    paywith:String
})

module.exports = mongoose.model('jobstatus',jobstatus);