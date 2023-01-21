function decodeBase64(str) {
  let output = '';
  let chr1, chr2, chr3;
  let enc1, enc2, enc3, enc4;
  let i = 0;
  str = str.replace(/[^A-Za-z0-9+/=]/g, '');
  while (i < str.length) {
    enc1 = this.keyStr.indexOf(str.charAt(i++));
    enc2 = this.keyStr.indexOf(str.charAt(i++));
    enc3 = this.keyStr.indexOf(str.charAt(i++));
    enc4 = this.keyStr.indexOf(str.charAt(i++));
    chr1 = (enc1 << 2) | (enc2 >> 4);
    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
    chr3 = ((enc3 & 3) << 6) | enc4;
    output = output + String.fromCharCode(chr1);
    if (enc3 != 64) {
      output = output + String.fromCharCode(chr2);
    }
    if (enc4 != 64) {
      output = output + String.fromCharCode(chr3);
    }
  }
  return output;
}

export {
    decodeBase64
}