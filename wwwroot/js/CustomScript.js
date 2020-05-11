function confirmDelete(uniqueID, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueID;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueID;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}