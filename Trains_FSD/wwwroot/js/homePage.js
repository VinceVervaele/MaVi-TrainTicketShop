$(document).ready(() => {
    $(".setLang").on("change", () => {
        $("form").submit();
    })
})

const translationIcon = $('#translationIcon');
const langSelector = $('#langSelector');
const iconButtons = $('#langSelector .iconButton');

translationIcon.on("click", function () {
    if (langSelector.hasClass('show')) {
        langSelector.removeClass('show');
        iconButtons.each(function (index) {
            $(this).css({
                'transition-delay': `${0.05 * ($iconButtons.length - index)}s`,
                'opacity': 0,
                'transform': 'translateY(20px)'
            });
        });
    } else {
        langSelector.addClass('show');
        iconButtons.each(function (index) {
            $(this).css({
                'transition-delay': `${0.05 * index}s`,
                'opacity': 1,
                'transform': 'translateY(0)'
            });
        });
    }
});
