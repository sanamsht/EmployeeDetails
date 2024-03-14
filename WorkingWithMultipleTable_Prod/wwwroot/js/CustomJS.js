

$('#txtSearch').keyup(function () {
    var typeValue = $(this).val();
    $('tbody tr').each(function () {
        if ($(this).text().search(new RegExp(typeValue, "i")) < 0) {
            $(this).fadeOut();
        }
        else {
            $(this).show();
        }
    });
});

$("input[type=checkbox]").on("click", function () {

    if ((this).checked) {
        $('#btnDeleteSelected').show();
    }
    else {
        $('#btnDeleteSelected').hide();
    }
});

$('#selectAll').on("click", function () {
    let checkboxes = document.querySelectorAll("input[type=checkbox]");
    let val = null;
    for (var i = 0; i < checkboxes.length ; i++) {

        if (val == null) {
            val = checkboxes[i].checked;
        }
        else {
            checkboxes[i].checked = val;
        }
    }
});


$('#btnDeleteSelected').on("click", function () {
   
    let val = [];
    $(':checkbox:checked').each(function (i) {
        val[i] = $(this).val();
    })
    
    $.ajax({
        type: "POST",
        url: "/Employee/Delete",
        data: { "id": val },
        success: function (response) {
            if (response != null) {
                location.reload();
            }
        },
        error: function () {

        }

    });
})