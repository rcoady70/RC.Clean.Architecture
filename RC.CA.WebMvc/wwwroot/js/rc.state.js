//Page state + utilities
//
var rcState = {
    hostUrl: "",
    antiForgeryToken: "",
    token: "Bearer ",
    debugScript: true,
    debugLog: function (type, message) {
        if (rcPage.debugScript)
            console.log("Debug::" + type + ' - ' + message);
    }
}
