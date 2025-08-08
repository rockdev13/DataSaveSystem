mergeInto(LibraryManager.library, {
    SyncFS: function () {
        FS.syncfs(false, function (err) {
            if (err) {
                console.log('FS.syncfs failed:', err);
            }
        });
    }
});