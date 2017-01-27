// JavaScript Document
$(window).on("scroll touchmove", function() {
  if ($(document).scrollTop() >= $("#one").position().top) {
    $('body').css('background', $("#one").attr("data-color"));
  };
  if ($(document).scrollTop() > $("#two").position().top) {
    $('body').css('background', $("#two").attr("data-color"))
  };
  if ($(document).scrollTop() > $("#three").position().top) {
    $('body').css('background', $("#three").attr("data-color"))
  };
  
 if ($(document).scrollTop() > $("#four").position().top) {
    $('body').css('background', $("#four").attr("data-color"))
  };
  
});

 $('.switch').hover(function() {
        $(this).find('.avg_words').hide();
        $(this).find('.avg_num').show();
    }, function() {
        $(this).find('.avg_num').hide();
        $(this).find('.avg_words').show();
});