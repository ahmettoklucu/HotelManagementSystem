function gp() {
    document.getElementById("Password").value = Password.generate(16);
}
var Password = {
 
  karakterler : /[a-zA-Z0-9_\-\+\.\*\/\&\?\!]/,
  
  
  sifreOlustur : function()
  {
    if(window.crypto && window.crypto.getRandomValues) 
    {
      var result = new Uint8Array(1);
      window.crypto.getRandomValues(result);
      return result[0];
    }
    else if(window.msCrypto && window.msCrypto.getRandomValues) 
    {
      var result = new Uint8Array(1);
      window.msCrypto.getRandomValues(result);
      return result[0];
    }
    else
    {
      return Math.floor(Math.random() * 256);
    }
  },
  
  generate : function(length)
  {
    return Array.apply(null, {'length': length})
      .map(function()
      {
        var result;
        while(true) 
        {
          result = String.fromCharCode(this.sifreOlustur());
          if(this.karakterler.test(result))
          {
            return result;
          }
        }        
      }, this)
      .join('');  
  }    
    
};