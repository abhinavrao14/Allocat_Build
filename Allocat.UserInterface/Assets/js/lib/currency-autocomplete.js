$(function(){
  var currencies = [
    
    { value: 'cancellous bone', data: 'KHR' },
    { value: 'cancellous bone graft', data: 'XAF' },
    { value: 'cancellous screw', data: 'CAD' },
    { value: 'cancellous crushed', data: 'CVE' },
    { value: 'crushed cancellous chips', data: 'KYD' },
    { value: 'crushed cancellous bone graft', data: 'CLP' },
    
  ];
  
  // setup autocomplete function pulling from currencies[] array
  $('#autocomplete').autocomplete({
    lookup: currencies,
    onSelect: function (suggestion) {
      var thehtml = '<strong>Currency Name:</strong> ' + suggestion.value + ' <br> <strong>Symbol:</strong> ' + suggestion.data;
      $('#outputcontent').html(thehtml);
    }
  });
  

});