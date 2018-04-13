$(function () {
    $("#btnAdd").live('click', function (event) {
        EditEntry(0);
    });

    $("#btnEdit").live('click', function (event) {
        var anSelected = fnGetSelected($('#resultTable').dataTable());
        if (anSelected.length !== 0) {
            EditEntry(parseInt($('#resultTable').dataTable().fnGetData(anSelected[0])[0]));
        } else {
            alert('Please select an item to edit.');
        }
    });

    $("#resultTable tbody").click(function (event) {

        $(oTable.fnSettings().aoData).each(function () {
            $(this.nTr).removeClass('row_selected');
        });
        $(event.target.parentNode).addClass('row_selected');
    });

    $("#resultTable tbody tr").live('dblclick', function (event) {
        var aPos = oTable.fnGetPosition(this);
        var aData = oTable.fnGetData(aPos);
        gIDNumber = aData[0];
        EditEntry(gIDNumber);
    });

    columndefs = [{ "bSearchable": false, "bVisible": false, "aTargets": [0] }];

    if (typeof (columndefExtension) == "function")
        columndefs = columndefExtension(columndefs);

    var oTable = $('#resultTable').dataTable({
        "aoColumnDefs": columndefs,
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bProcessing": true,
        "bAutoWidth": false,
        "bServerSide": true,
        "iDisplayLength": 25,
        "sAjaxSource": AjaxSource
    }
    );
});

function fnGetSelected(oTableLocal) {
    return oTableLocal.$('tr.row_selected');
};