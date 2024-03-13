// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function DisplayLoader() {
    $('#Loader').show();
}
function HideLoader() {
    $('#Loader').hide();
}

$('#Loader').hide();

$(document).on('submit', 'form', function () {
    DisplayLoader();
})

$(window).on('beforeunload', function () {
    DisplayLoader();
})


$(function () {
    if ($('div.alert.notification').length) {
        setTimeout(() => {
            $('div.alert.notification').fadeOut();
        }, 3000)
    }
});

function hideNotification() {
    $('div.alert.notification').fadeOut();
}