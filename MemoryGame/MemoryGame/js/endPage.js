function playAudio(path) {
    var myAudio = new Audio;
    myAudio.src = path;
    myAudio.play();
}
$(function () {
    var select = getParametr("value");
    var txt = $('.endText').html();
    txt += select;
    $('.endText').html(txt);
    playAudio('./audio/congratulations.wav');
});
function getParametr(name) {
    var parametr = location.search.substring(1).split("&");
    var variable = "";
    for (var i = 0; i < parametr.length; i++) {
        if (parametr[i].split("=")[0] == name) {
            if (parametr[i].split("=").length > 1)
                variable = parametr[i].split("=")[1];
            return variable;
        }
    }
    return "";
}