$(document).ready(function () {
    debugger;
    var apiEndPoint = "";
    //Check if import id is set if not new
    if ($("#Id")[0].value.length == 0)
        $("#btnNext").addClass("d-none");

    if ($("#dropSection").length > 0)
        var apiEndPoint = $("#dropSection").data('apiurl');
    $("#dropSection").filedrop({
        fallback_id: 'btnUpload',
        fallback_dropzoneClick: true,
        url: apiEndPoint + '/api/csvfile/upload',
        allowedfiletypes: ['text/plain', 'application/vnd.ms-excel'],
        allowedfileextensions: ['.txt', '.csv'],
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
            norFileName = file.name.replace(/[^a-z0-9]/gi, '');
            if (response.totalErrors > 0) {
                $(response.errors).each(function (key, value) {
                    $('#uploadedFiles').append(uploadTemplate(norFileName, true));
                });
                rcPage.displayAjaxErrors(response);
            }
            else {
                $('#uploadedFiles').append(uploadTemplate(norFileName, false));
                $('#dropSection').addClass('upload-finished');
                $("#dropSection").unbind();
                $("#Id")[0].value = response.id;
                $("#btnNext").removeClass("d-none");
                $("#btnNext").addClass("d-block");
            }
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
                        <img class="img-fluid img-thumbnail upload-img" src="https://rpcstorageacc.blob.core.windows.net/images/CSVUploadImage06a620d698c64adcb35b38729e605a7b.jpg" name="` + imageId + `">
                        <div class="text-danger">failed "` + imageId + `"</div>
                    </div>`;
        else
            return `<div>
                        <img class="img-fluid img-thumbnail upload-img" src="https://rpcstorageacc.blob.core.windows.net/images/CSVUploadImage06a620d698c64adcb35b38729e605a7b.jpg" name="` + imageId + `">
                        <div class="text-success">uploaded "` + imageId + `"</div>
                    </div>`;
    }
});