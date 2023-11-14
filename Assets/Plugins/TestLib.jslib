mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(UTF8ToString(str));
    myGameInstance.SendMessage('Cube', 'DestroyCube');
  },

  PrintFloatArray: function (array, size) {
    for(var i = 0; i < size; i++)
    console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },

  GetCachedFiles: function () {
    var openRequest = indexedDB.open('UnityCache');

    openRequest.onsuccess = function(event) {
      var db = event.target.result;

      var transaction = db.transaction('FILE_DATA', 'readwrite');
      var objectStore = transaction.objectStore('FILE_DATA');
      var getRequest = objectStore.get('/Build/UnityLoader.js');

      getRequest.onsuccess = function(event) {
          if (event.target.result) {
              console.log('UnityLoader.js is cached');
          } else {
              console.log('UnityLoader.js is not cached');
          }
      };
    };
  
  },

});