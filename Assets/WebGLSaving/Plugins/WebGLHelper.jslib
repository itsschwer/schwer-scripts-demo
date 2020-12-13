mergeInto(LibraryManager.library, {
    // SyncFiles : function() {
    //     FS.syncfs(false, function(err) {
    //         // handle callback
    //     });
    // },
    SetDownload : function(base64, fileName) {
        // Reference: https://stackoverflow.com/questions/34339593/open-base64-in-new-tab
        var url = 'data:application/octet-stream;base64,' + Pointer_stringify(base64);
        var download = document.getElementById('download');
        download.href = url;
        download.download = Pointer_stringify(fileName);

        var time = new Date().toLocaleTimeString('en-GB', {hour: '2-digit', minute: '2-digit'});
        download.innerText = "Download save file (" + time + ")";

        download.classList.add("enabled");
    },
    ImportEnabled : function(enabled) {
        var e = document.getElementById('import');
        if (enabled) {
            e.classList.add("enabled");
        }
        else {
            e.classList.remove("enabled");
        }
    }
});
