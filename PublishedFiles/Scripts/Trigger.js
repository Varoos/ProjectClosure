var compID;
var userID = 0;
var userName = "";
var Docno = "";
var Vt = 0;
var requestId = 0;
var requestsProcessed = [];
var bodyRequestsProcessed = [];
var logDetails = {};
var HeaderRequestsProcessed = {};
var xUrl = 'http://localhost/PrjProjectClosure/';

var ServerIPAddress = {
    ServerIP: 'http://localhost/PrjProjectClosure/'
};

function isRequestProcessed(iRequestId) {
    for (let i = 0; i < requestsProcessed.length; i++) {
        if (requestsProcessed[i] === iRequestId) {
            return true;
        }
    } return false;
}
//Get Session and Login
function getSetPropertyForCompanyId() {
    debugger
    Focus8WAPI.getGlobalValue("fnGetValueCallBack", "*", 2);
}
let r = 1
function fnGetValueCallBack(objWrapperResult) {
    debugger
    try {
        console.log(`fnGetValueCallBack :: ${r} `, objWrapperResult);
        var responseData = objWrapperResult.data;
        console.log(responseData.SessionId);
        userID = responseData.LoginId;
        userName = responseData.UserName;
        compID = responseData.CompanyId;
    }
    catch (err) {
        alert("Exception:" + err.message);
    }
}
function BeforeSaveFn() {
    debugger;
    getSetPropertyForCompanyId();
    Focus8WAPI.getBodyFieldValue('RetriveValues', ["", "Project"], 2, false, 1, requestId++);
}
function setBodyFieldValueCallBack(response) {
    debugger
    requestsProcessed.push(response.iRequestId);
    console.log('Callback :: setBodyFieldValueCallBack :: Response : ', response);
}
function setAfterCallback(response) {
    debugger
    if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
        return;
    }
    requestsProcessed.push(response.iRequestId);


    console.log(response);
    logDetails = response.data[0];
    docNo = response.data[1].FieldValue;
    HeaderRequestsProcessed = { "VoucherType": logDetails.iVoucherType, "DocNo": docNo, "Date": response.data[2].FieldValue };
}

function RetriveValues(response) {
    debugger;
    if (isRequestCompleted(response.iRequestId, requestsProcessed)) {
        return;
    }
    requestsProcessed.push(response.iRequestId);
    isvalidrows = response.RowsInfo.iValidRows;
    vsBodyData = {};
    vsBodyDataArr = [];
    vsBodyDataArr.length = 0;
    bodyRequestsProcessed = [];
    if (isvalidrows > 0) {

        for (let i = 1; i <= isvalidrows; i++) {
            Focus8WAPI.getBodyFieldValue('initializeRow', ["*"], Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, false, i, i);
        }

    }
}

function isRequestCompleted(iRequestId, requestsArray) {
    debugger;
    requestsArray.indexOf(iRequestId) === -1 ? false : true;
}
function initializeRow(response) {
    debugger;
    try {
        if (isRequestCompleted(response.iRequestId, bodyRequestsProcessed)) {
            return;
        }
        bodyRequestsProcessed.push(response.iRequestId);

        const row = initializeRowDataFields(response.data.slice(0));
        vsBodyDataArr.push(row);
        vsBodyData[response.iRequestId] = row;
        if (isvalidrows === Object.values(vsBodyData).length) {
            debugger;
            ValidateData();
        }


    } catch (e) {
        alert(e.message)
    }
}

function ValidateData() {
    debugger;
    console.log("Entered Validation");
    var prjcode = '';
    for (var i = 0; i < vsBodyDataArr.length; i++) {
        debugger;
        var prj = (vsBodyDataArr[i].Project.FieldValue);
        prjcode = (vsBodyDataArr[i].Project.FieldText);
        if (prj != '') {
            console.log("Validate i loop prj = " + prj);
            $.ajax({
                type: "get",
                url: xUrl + "Home/Index?CompId=" + compID + "&ProjectId=" + prj,
                datatype: "JSON",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    debugger;
                    console.log("Success");
                    console.log(result);
                    var status = result;
                    if (status == '1') {
                        alert("PROJECT STATUS IS CLOSED FOR " + prjcode);
                        $('#id_transactionentry_save').attr('style', 'pointer-events: auto');
                        return false;
                    }
                    else {
                        Focus8WAPI.continueModule(Focus8WAPI.ENUMS.MODULE_TYPE.TRANSACTION, true);
                    }
                },
                error: function (err) {
                    console.log("Error");
                    console.log(err);
                }
            });
        }
    }
}
function initializeRowDataFields(fields) {
    debugger;
    const row = {};
    Object.values(fields).forEach((v, i, a) => {
        if (v) {
            row[`${v['sFieldName']}`] = {};
            row[`${v['sFieldName']}`]['sFieldName'] = v['sFieldName'];
            row[`${v['sFieldName']}`]['FieldText'] = v['FieldText'];
            row[`${v['sFieldName']}`]['FieldValue'] = v['FieldValue'];
            row[`${v['sFieldName']}`]['iFieldId'] = v['iFieldId'];
        }

    })
    console.log('rowDataM   string:: ', JSON.stringify(row));
    return row;
}


