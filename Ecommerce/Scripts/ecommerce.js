$(document).ready(function () {
    $("#DepartmentID").change(function() {
        $("#CityID").empty();
        $("#CityID").append('<option value="0">[Select a city...]</option>');
        $.ajax({
            type: 'POST',
            url: Url,
            dataType: 'json',
            data: { departmentId: $("#DepartmentID").val() },
            success: function(data) {
                $.each(data, function(i, data) {
                    $("#CityID").append('<option value="'
                        + data.CityID + '">'
                        + data.Name + '</option>');
                });
            },
            error: function(ex) {
                alert('Failed to retrieve cities.' + ex);
            }
        });
        return false;
    })
});
