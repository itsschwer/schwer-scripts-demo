// `Pointer_stringify` should be replaced with `UTF8ToString` from Unity 2021.2 onwards!
mergeInto(LibraryManager.library, {
    Export: function (base64, fileName) {
        // Reference: https://stackoverflow.com/questions/34339593/open-base64-in-new-tab
        const url = 'data:application/octet-stream;base64,' + Pointer_stringify(base64);

        const dl = document.createElement('a');
        dl.href = url;
        dl.download = Pointer_stringify(fileName);
        dl.click();
        dl.remove();
    },

    Import: function (extension, receiverObject, receiverMethod) {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = Pointer_stringify(extension);

        // https://stackoverflow.com/questions/44891748/pointer-stringify-is-returning-garbled-text
        const object = Pointer_stringify(receiverObject);
        const method = Pointer_stringify(receiverMethod);

        input.oninput = function (e) {
            const files = e.target.files;

            if (files.length === 0) {
                input.remove();
                return;
            }

            const fileReader = new FileReader();
            fileReader.readAsArrayBuffer(files[0]);
            fileReader.onload = function () {
                // Reference: https://stackoverflow.com/questions/17845032/net-mvc-deserialize-byte-array-from-json-uint8array
                const str = String.fromCharCode.apply(null, new Uint8Array(fileReader.result));
                SendMessage(object, method, window.btoa(str));

                input.remove();
            };
        }

        input.click();
    },
});
