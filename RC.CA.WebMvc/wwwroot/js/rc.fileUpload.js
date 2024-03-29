﻿$(function () {
    //alert("init");
    var apiEndPoint ="";
    if ($("#dropSection").length > 0)
        var apiEndPoint = $("#dropSection").data('apiurl');
    $("#dropSection").filedrop({
        fallback_id: 'btnUpload',
        fallback_dropzoneClick: true,
        url: apiEndPoint + '/api/cdn/Image/Upload',
        allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif'],
        allowedfileextensions: ['.jpg', '.jpeg', '.png', '.gif'],
        paramname: 'fileData',
        headers: { 'Authorization': '' },
        maxfiles: 10, //Maximum Number of Files allowed at a time.
        maxfilesize: 2, //Maximum File Size in MB.
        dragOver: function () {
            $('#dropSection').addClass('upload-active');
        },
        dragLeave: function () {
            $('#dropSection').removeClass('upload-active');
        },
        drop: function () {
            $('#dropSection').removeClass('upload-active');
        },
        uploadFinished: function (i, file, response, time) {
            debugger;
            console.log(response);
            norFileName = file.name.replace(/[^a-z0-9]/gi, '');
            if (!response.isSuccess) {
                $(response.errors).each(function (key, value) {
                    $('#uploadedFiles').append(uploadTemplate(norFileName, true));
                });
                rcPage.displayAjaxErrors(response);
            }
            else
                $('#uploadedFiles').append(uploadTemplate(norFileName, false));
            $('#' + norFileName)[0].src = window.URL.createObjectURL(file);
        },
        afterAll: function (e) {

            //refresh view
            var parms = {
                filterByName: $("#FilterByName")[0].value,
                filterById: "",
                OrderBy: "createdon_desc",
                pageSeq: 1,
            };

            //Refresh images after image upload
            rcAjaxServices.getImages(parms)
                .done(function (result, status, xhr) {
                    $("#imageListContainer").html(result)
                        rcList.unBindListActions();
                        rcList.initListActions();
                })
                .fail(function (xhr, status, error) {
                    rcPage.toastError("Refresh failed: " + status + " " + error + " " + xhr.status + " " + xhr.statusText, true);
                });
        }
    })
    function uploadTemplate(imageId, failed) {
        if (failed)
            return `<div>
                        <img class="img-fluid img-thumbnail upload-img" id="`+ imageId + `" name="` + imageId +`">
                        <div class="text-danger">failed</div>
                    </div>`;
        else
            return `<div>
                        <img class="img-fluid img-thumbnail upload-img" id="`+ imageId + `" name="` + imageId + `">
                        <div class="text-success">uploaded</div>
                    </div>`;
    }
})