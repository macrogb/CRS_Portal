// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//************** Run Audit Page JS **************//
//BEGIN
//END


//************** Run Audit Page JS **************//
//BEGIN
//For A_B_C_D file


function ajaxPost4All(formData) {
    
    $.ajax(
        {
            url: $("#btnRunAudit").data('url'),
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (result) {
                console.log(result);
                if (result.success == true) {
                    alertify.alert(result.message);
                    return false;
                }
                else {
                    alertify.alert(result.message);
                    return false;
                }
            }
        }
    );
}
//END



function clearOldRecordsFromTables(val) {
    if (val =="tbAutomationAppConfig") {
         $("#tbAutomationAppConfig tbody").empty();
    }
    else if (val == "tbAutomationMailConfig") {
        $("#tbAutomationMailConfig tbody").empty();
    }
    else if (val == "tbAutomationStatusConfig") {
        $("#tbAutomationStatusConfig tbody").empty();
    }
    else if (val == "tbAutomationStyleConfig") {
        $("#tbAutomationStyleConfig tbody").empty();
    }
    else if (val == "tbAutomationFileConfig") {
        $("#tbAutomationFileConfig tbody").empty();
    }
    else if (val == "tbAutomationPCAConfig") {
        $("#tbAutomationPCAConfig tbody").empty();
    }
    else if (val == "tbClientAdmnBankData") {
        $("#tbClientAdmnBankData tbody").empty();
    }
    else if (val == "tbClientAdmnProdDet") {
        $("#tbClientAdmnProdDet tbody").empty();
    }
}

function LoadValuesToTalble(val, tr) {
   
    if (val == "tbAutomationAppConfig") {
        $("#tbAutomationAppConfig").append(tr);
    }
    else if (val == "tbAutomationMailConfig") {
        $("#tbAutomationMailConfig").append(tr);
    }
    else if (val == "tbAutomationStatusConfig") {
        $("#tbAutomationStatusConfig").append(tr);
    }
    else if (val == "tbAutomationStyleConfig") {
        $("#tbAutomationStyleConfig").append(tr);
    }
    else if (val == "tbAutomationFileConfig") {
        $("#tbAutomationFileConfig").append(tr);
    }
    else if (val == "tbAutomationPCAConfig") {
        $("#tbAutomationPCAConfig").append(tr);
    }
    else if (val == "tbClientAdmnBankData") {
        $("#tbClientAdmnBankData").append(tr);
    }
    else if (val == "tbClientAdmnProdDet") {
        $("#tbClientAdmnProdDet").append(tr);
    }
}

function LoadPolmtpfGridDatas(filter, pageIndex, tableName) {
    $.ajax(
        {
            type: 'GET',
            data: { "moduleName": filter, "pageIndex": pageIndex },
            url: $("#btnFilter").data('url'),
            success: function (json) {
                var tr;
                if (json == null) {
                    return false;
                }
                else {
                    clearOldRecordsFromTables(tableName);

                    for (var i = 0; i < json.length; i++) {
                        //var actionIcon = "<i class='fas fa-edit' id='polEdit' style='color: orange;cursor:pointer' onclick='loadModalPopup_Polmtpf(thisValue);'></i> &nbsp; <i class='fas fa-trash' style='color: orangered'></i>";
                        var actionIcon = "<i class='fas fa-edit' id='polEdit' style='color: #000;cursor:pointer' onclick='loadModalPopup_Polmtpf(thisValue);'></i>";
                        actionIcon = actionIcon.replace("thisValue", json[i].ID);
                        actionIcon = actionIcon.replace("polEdit", "polEdit_" + json[i].ID);

                        tr = $('<tr/>');
                        tr.append("<td style='display: none'>" + json[i].ID + "</td>");
                        tr.append("<td>" + json[i].PMODULE + "</td>");
                        tr.append("<td>" + json[i].PTYPE + "</td>");
                        tr.append("<td>" + json[i].PDESC + "</td>");
                        tr.append("<td>" + json[i].POLDET + "</td>");
                        tr.append("<td>" + json[i].PEDTYP + "</td>");
                        tr.append("<td>" + actionIcon + "</td>");
                        LoadValuesToTalble(tableName, tr);
                    }
                }
            },
            error: function (data) {
                console.log(data); //alert(data)
            }
        });
}

function hideModal2() {
    $("#modal2").removeClass("in");
    $(".modal-backdrop").remove();
    $('body').removeClass('modal-open');
    $("#modal2").hide();
    return false;
}
function showModal2() {
    $('body').addClass('modal-open');
    $("#modal2").addClass("in");
    $("#modal2").css("display", "block");
    $("#modal2").show();
    return false;
}

function spinner_On() {
    $('.spinner').css('display', 'block');
}
function spinner_Off() {
    $('.spinner').css('display', 'none');
}

//File upload Js -- Start
function _(el) {
    return document.getElementById(el);
}

function formatBytes(bytes, decimals) {
    if (bytes == 0) return '0 Bytes';
    var k = 1024,
        dm = decimals <= 0 ? 0 : decimals || 2,
        sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
        i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}
function completeHandler(event) {
    // _("status").innerHTML = event.target.responseText;
}

function errorHandler(event) {
    //_("status").innerHTML = "Upload Failed";
    alertify.alert("SCV Portal", "Upload Failed");
}

function abortHandler(event) {
    //_("status").innerHTML = "Upload Aborted";
    alertify.alert("SCV Portal", "Upload Aborted");
}

function progressHandler(event) {
    $("#fileUploadPgbar").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar").css("width", percent + "%");
    $("#progressBar").text(percent+"%");
    if (percent== "100" ) {
        $("#file_upload_SCV_1_ABCD").prop("disabled", false);
        $("#file_upload_SCV_2_ABD").prop("disabled", false);
        $("#file_upload_SCV_4_A").prop("disabled", false);

        $("#file_upload_SCV_1_ABCD").css("opacity", "0");
        $("#file_upload_SCV_2_ABD").css("opacity", "0");
        $("#file_upload_SCV_4_A").css("opacity", "0");
    }
}

function progressHandler_1(event) {
    $("#fileUploadPgbar_1").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_1").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_1").css("width", percent + "%");
    $("#progressBar_1").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_1_ABCD").prop("disabled", false);
        $("#file_upload_SCV_2_C").prop("disabled", false);
        $("#file_upload_SCV_4_B").prop("disabled", false);

        $("#file_upload_EXC_1_ABCD").css("opacity", "0");
        $("#file_upload_SCV_2_C").css("opacity", "0");
        $("#file_upload_SCV_4_B").css("opacity", "0");
    }
}

function progressHandler_2(event) {
    $("#fileUploadPgbar_2").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_2").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_2").css("width", percent + "%");
    $("#progressBar_2").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_2_ABD").prop("disabled", false);
        $("#file_upload_SCV_4_C").prop("disabled", false);

        $("#file_upload_EXC_2_ABD").css("opacity", "0");
        $("#file_upload_SCV_4_C").css("opacity", "0");
    }
}

function progressHandler_3(event) {
    $("#fileUploadPgbar_3").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_3").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_3").css("width", percent + "%");
    $("#progressBar_3").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_2_C").prop("disabled", false);
        $("#file_upload_SCV_4_D").prop("disabled", false);

        $("#file_upload_EXC_2_C").css("opacity", "0");
        $("#file_upload_SCV_4_D").css("opacity", "0");
    }
}

function progressHandler_4(event) {
    $("#fileUploadPgbar_4").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_4").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_4").css("width", percent + "%");
    $("#progressBar_4").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_4_A").prop("disabled", false);
        $("#file_upload_EXC_4_A").css("opacity", "0");
    }
}

function progressHandler_5(event) {
    $("#fileUploadPgbar_5").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_5").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_5").css("width", percent + "%");
    $("#progressBar_5").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_4_B").prop("disabled", false);
        $("#file_upload_EXC_4_B").css("opacity", "0");
    }
}

function progressHandler_6(event) {
    $("#fileUploadPgbar_6").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_6").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_6").css("width", percent + "%");
    $("#progressBar_6").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_4_C").prop("disabled", false);
        $("#file_upload_EXC_4_C").css("opacity", "0");
    }
}

function progressHandler_7(event) {
    $("#fileUploadPgbar_7").show();
    var percent = Math.round((event.loaded / event.total) * 100);
    $("#progressBar_7").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_7").css("width", percent + "%");
    $("#progressBar_7").text(percent + "%");
    if (percent == "100") {
        $("#file_upload_EXC_4_D").prop("disabled", false);
        $("#file_upload_EXC_4_D").css("opacity", "0");
    }
}

//File upload Js -- End

function hideSuccessErrorDiv() {
    $("#divError").hide();
    $("#divSuccess").hide();
}

function enableSucessDiv(message) {
    $("#divSuccess").show();
    $("#divSuccessMess").html(message);
    $("#divError").hide();
}

function enableErrorDiv(message) {
    $("#divError").show();
    $("#divErrorMess").html(message);
    $("#divSuccess").hide();
}

function ResetFileUplod() {
    $('#fileName').html("");
    var percent = 0;
    $("#progressBar").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar").css("width", percent + "%");
    $("#progressBar").text(percent);
    $("#fileUploadPgbar").hide();
}

function ResetFileUplod_1() {
    $('#fileName_1').html("");
    var percent = 0;
    $("#progressBar_1").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_1").css("width", percent + "%");
    $("#progressBar_1").text(percent);
    $("#fileUploadPgbar_1").hide();
}

function ResetFileUplod_2() {
    $('#fileName_2').html("");
    var percent = 0;
    $("#progressBar_2").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_2").css("width", percent + "%");
    $("#progressBar_2").text(percent);
    $("#fileUploadPgbar_2").hide();
}

function ResetFileUplod_3() {
    $('#fileName_3').html("");
    var percent = 0;
    $("#progressBar_3").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_3").css("width", percent + "%");
    $("#progressBar_3").text(percent);
    $("#fileUploadPgbar_3").hide();
}

function ResetFileUplod_4() {
    $('#fileName_4').html("");
    var percent = 0;
    $("#progressBar_4").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_4").css("width", percent + "%");
    $("#progressBar_4").text(percent);
    $("#fileUploadPgbar_4").hide();
}

function ResetFileUplod_5() {
    $('#fileName_5').html("");
    var percent = 0;
    $("#progressBar_5").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_5").css("width", percent + "%");
    $("#progressBar_5").text(percent);
    $("#fileUploadPgbar_5").hide();
}

function ResetFileUplod_6() {
    $('#fileName_6').html("");
    var percent = 0;
    $("#progressBar_6").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_6").css("width", percent + "%");
    $("#progressBar_6").text(percent);
    $("#fileUploadPgbar_6").hide();
}

function ResetFileUplod_7() {
    $('#fileName_7').html("");
    var percent = 0;
    $("#progressBar_7").css("aria-valuenow", "<b>" + percent + "</b>");
    $("#progressBar_7").css("width", percent + "%");
    $("#progressBar_7").text(percent);
    $("#fileUploadPgbar_7").hide();
}



//Close Div Add/Edit/Bulk upload Div's
$('.clsDv').on("click", function () {
    $(this).parent().parent().parent().parent().hide();
});


function LoadAuditValues() {
    $.ajax(
        {
            type: 'GET',
            data: "",
            url: $("#btnRunAuditEdit").data('url'),
            success: function (json) {
                //console.log(json);
                $('#SCVLicenseActivatedDate').text(json.SCVLicenseActivatedDate);
                $('#SCVLicenseExpiryDate').text(json.SCVLicenseExpiryDate);
                $('#RemainingDays').text(json.RemainingDays);
                $('#TotalSCVAuditCount').text(json.TotalSCVAuditCount);
                $('#CompletedSCVAuditCount').text(json.CompletedSCVAuditCount);
                $('#RemainingSCVAuditCount').text(json.RemainingSCVAuditCount);
                $('#imgCaptcha').attr('src', json.CapImage);
                $("#txtCaptcha").val(json.capCodeText);
                if (json.FileType == 1) {
                    if (json.ProcSubFileType == 1) { //both
                        $('#dvSCVABCDFilename').show();
                        $('#SCV_ABCD_Filename').text(json.SCV_ABCD_Filename);
                        $('#dvEXCABCDFilename').show();
                        $('#EXC_ABCD_Filename').text(json.EXC_ABCD_Filename);                        
                    }
                    else if (json.ProcSubFileType == 2) { //SCV
                        $('#dvSCVABCDFilename').show();
                        $('#SCV_ABCD_Filename').text(json.SCV_ABCD_Filename);
                        $('#dvEXCABCDFilename').hide();
                    }
                    else { //Exclusion
                        $('#dvSCVABCDFilename').hide();
                        $('#dvEXCABCDFilename').show();
                        $('#EXC_ABCD_Filename').text(json.EXC_ABCD_Filename);
                    }                   
                }
                else if (json.FileType == 2) {
                    if (json.ProcSubFileType == 1) { //both
                        $('.dvSCVABDFilename').show();
                        $('#SCV_ABD_Filename').text(json.SCV_ABD_Filename);
                        $('#SCV_C_Filename').text(json.SCV_C_Filename);
                        $('.dvEXCABDFilename').show();
                        $('#EXC_ABD_Filename').text(json.EXC_ABD_Filename);
                        $('#EXC_C_Filename').text(json.EXC_C_Filename);
                    }
                    else if (json.ProcSubFileType == 2) { //SCV
                        $('.dvSCVABDFilename').show();
                        $('#SCV_ABD_Filename').text(json.SCV_ABD_Filename);
                        $('#SCV_C_Filename').text(json.SCV_C_Filename);
                        $('.dvEXCABDFilename').hide();                        
                    }
                    else { //Exclusion
                        $('.dvSCVABDFilename').hide();
                        $('.dvEXCABDFilename').show();
                        $('#EXC_ABD_Filename').text(json.EXC_ABD_Filename);
                        $('#EXC_C_Filename').text(json.EXC_C_Filename);
                    }
                }
                else if (json.FileType == 4) {
                    if (json.ProcSubFileType == 1) { //both
                        $('.dvSCV_A_B_C_D_Filename').show();
                        $('#SCV_A_Filename').text(json.SCV_A_Filename);
                        $('#SCV_B_Filename').text(json.SCV_B_Filename);
                        $('#SCV_C_Filename').text(json.SCV_C_Filename);
                        $('#SCV_D_Filename').text(json.SCV_D_Filename);

                        $('.dvEXC_A_B_C_D_Filename').show();
                        $('#EXC_A_Filename').text(json.EXC_A_Filename);
                        $('#EXC_B_Filename').text(json.EXC_B_Filename);
                        $('#EXC_C_Filename').text(json.EXC_C_Filename);
                        $('#EXC_D_Filename').text(json.EXC_D_Filename);                        
                    }
                    else if (json.ProcSubFileType == 2) { //SCV
                        $('.dvSCV_A_B_C_D_Filename').show();
                        $('#SCV_A_Filename').text(json.SCV_A_Filename);
                        $('#SCV_B_Filename').text(json.SCV_B_Filename);
                        $('#SCV_C_Filename').text(json.SCV_C_Filename);
                        $('#SCV_D_Filename').text(json.SCV_D_Filename);
                        $('.dvEXC_A_B_C_D_Filename').hide();                        
                    }
                    else { //Exclusion
                        $('.dvSCV_A_B_C_D_Filename').hide();
                        $('.dvEXC_A_B_C_D_Filename').show();
                        $('#EXC_A_Filename').text(json.EXC_A_Filename);
                        $('#EXC_B_Filename').text(json.EXC_B_Filename);
                        $('#EXC_C_Filename').text(json.EXC_C_Filename);
                        $('#EXC_D_Filename').text(json.EXC_D_Filename);
                    }
                }
                spinner_Off();
            }
            
        });
}