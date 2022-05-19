$(function () {
    var apiEndPoint ="";
    if ($("#dropCsvSection").length > 0)
        var apiEndPoint = $("#dropCsvSection").data('apiurl');

    $("#dropCsvSection").filedrop({
        fallback_id: 'btnUpload',
        fallback_dropzoneClick: true,
        url: apiEndPoint + '/api/cdn/csvfile/Upload',
        
        allowedfileextensions: ['.txt', '.csv'],
        paramname: 'fileData',
        headers: { 'Authorization': '' },
        maxfiles: 1, //Maximum Number of Files allowed at a time.
        maxfilesize: 2, //Maximum File Size in MB.
        dragOver: function () {
            $('#dropCsvSection').addClass('upload-active');
        },
        dragLeave: function () {
            $('#dropCsvSection').removeClass('upload-active');
        },
        drop: function () {
            $('#dropCsvSection').removeClass('upload-active');
        },
        uploadFinished: function (i, file, response, time) {
            debugger;
            alert("Uploaded");
            //console.log(response);
            //norFileName = file.name.replace(/[^a-z0-9]/gi, '');
            //if (response.totalErrors > 0) {
            //    $(response.errors).each(function (key, value) {
            //        $('#uploadedFiles').append(uploadTemplate(norFileName, true));
            //    });
            //    rcPage.displayAjaxErrors(response);
            //}
            //else
            //    $('#uploadedFiles').append(uploadTemplate(norFileName, false));
            //$('#' + norFileName)[0].src = window.URL.createObjectURL(file);
        },
        afterAll: function (e) {
            //Move to the next step
        }
    })
    function uploadTemplate(id, failed) {
        if (failed)
            return `<div>
                        <img class="img-fluid img-thumbnail upload-img" id="`+ id + `" name="` + id +`">
                        <div class="text-danger">failed</div>
                    </div>`;
        else
            return `<div>
                        <img class="img-fluid img-thumbnail upload-img" id="`+ id + `" name="` + id + `">
                        <div class="text-success">uploaded</div>
                    </div>`;
    }
})