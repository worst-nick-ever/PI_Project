(function ($) {
    $.validator.addMethod("beforetoday", function (value, element, params) {
        var dateValue = Date.parse(value);
        if (dateValue != NaN) {
            return dateValue < Date.now();
        }
        return false;
    });
    $.validator.unobtrusive.adapters.addBool("beforetoday");

}(jQuery));