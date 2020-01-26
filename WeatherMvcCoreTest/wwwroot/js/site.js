// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.myButton').hide();
    $("#Name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Weather/Weather",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, value: item.name };
                    }))

                }
            })
        },
        messages: {
            noResults: "No results",
            results: function (count) {
                return count + (count == 0 ? ' result' : ' results');
            }
        }
    });
})
$(document).ready(function () {
    $("#Name").on('input', function () {
        $('#city').val($(this).val());
        $('.myButton').show();
    });
});