function getQueryStringByName(name) {
  var result = window.localStorage.getItem('search')?.match(new RegExp('[?&]' + name + '=([^&]+)', 'i'));

  if (result == null || result.length < 1) {
    return '';
  }
  return result[1];
}

export { getQueryStringByName };
