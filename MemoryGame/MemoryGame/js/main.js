
var countOfPlaces = 18;         // количество мест на поле (т.е. карт, которые будут участвовать в игре)

var indexOfSelectedCards = [];  // в массиве будут храниться выбранные случайным образом 18 карт;
                                // элементы массива indexOfSelectedCards[] могут принимать значения от 0 до 51 включительно


var indexOfSelectedPlaces = []; // в массиве будут храниться номера выбранных случайным образом мест на поле для каждого
                                // номера карты, хранящегося по соответствующему индексу в массиве indexOfSelectedCards[];
                                // элементы массива indexOfSelectedPlaces[] могут принимать значения от 0 до 17 включительно

var arrayOfImages = [];         // в массиве будут храниться значения атрибута src для каждой из 52 карт

$(function () {

    // динамическое добавление контейнеров для каждой карты  
    for (var i = 0; i < countOfPlaces; i++) {
        $(".cards").append("<div class=card id=c" + i + "><img src='' id=" + i + "/></div>");
    }

    // заполнение массива arrayOfImages[] с помощью функции getCards(arrayOfImages,letter)
    getCards(arrayOfImages, "C");
    getCards(arrayOfImages, "D");
    getCards(arrayOfImages, "H");
    getCards(arrayOfImages, "S");
   
    // случайный выбор номеров 9 карт с помощью функции getRandomNumber(min, max, randomArr)
    while (indexOfSelectedCards.length != countOfPlaces / 2) {
        getRandomNumber(0, 51, indexOfSelectedCards);
    }

    // выбирается только 9 карт, для того чтобы у каждой карты была пара

    // затем выбранные случайным образом номера 9 карт еще раз помещаются в массив indexOfSelectedCards[]
    for (var i = 0; i < countOfPlaces / 2; i++) {
        indexOfSelectedCards.push(indexOfSelectedCards[i]);
    }

    // пока для каждой из 18 выбранных карт не будет определено случайное место на поле,
    // вызываем функцию getRandomNumber(min, max, randomArr)
    while (indexOfSelectedPlaces.length != countOfPlaces) {
        getRandomNumber(0, 17, indexOfSelectedPlaces);
    }

    // задаем элементам img на игровом поле атрибуты src выбранных картинок
    for (var i = 0; i < countOfPlaces; i++) {
        $("#c" + indexOfSelectedPlaces[i] + " img").attr('src', arrayOfImages[indexOfSelectedCards[i]]);
        $("#c" + indexOfSelectedPlaces[i] + " img").attr('data-tid', 'Card');
    }

    setTimeout(changeCards, 5000); // переворачиваем карты рубашкой вверх через 5 секунд
})

var path = "./images/cards/";
var format = ".png";

// функция добавления значений атрибутов src картинок из папки ./images/cards/ в массив arrayOfImages[]
// letter - масть карты
function getCards(arrayOfImages,letter) {
    for (var i = 0; i < 10; i++) {
        if (i == 1)
            continue;
        var src = path + i + letter + format;
        arrayOfImages.push(src);
    }
    // вызов вспомогательной функции для каждой масти карт
    getCardsSub(arrayOfImages, letter, "A");
    getCardsSub(arrayOfImages, letter, "J");
    getCardsSub(arrayOfImages, letter, "K");
    getCardsSub(arrayOfImages, letter, "Q");
}

// вспомогательная функция для добавления путей к тузам, вальтам, дамам и королям
// letter - масть карты, letter1 - A, J, K или Q
function getCardsSub(arrayOfImages, letter, letter1) {
    var src = path + letter1 + letter + format;
    arrayOfImages.push(src);
}

// функция генерации случайного числа в диапазоне от min до max включительно
function getRandomNumber(min, max, randomArr) {
    var randNumber = Math.floor(Math.random() * (max - min + 1)) + min;

    // проверка, было ли сгенерировано данное число ранее
    if (randomArr.indexOf(randNumber) == -1)
        randomArr.push(randNumber);
}

var index;                  // индекс текущей кликнутой карты в массивах indexOfSelectedPlaces[] и indexOfSelectedCards[]
var indexPrev;              // индекс первой открытой за ход карты в массивах indexOfSelectedPlaces[] и indexOfSelectedCards[]

var flag = true;            // при равенстве true флаг показывает, что была открыта первая за ход карта
var scoreValue = 0;         // число набранных очков
var numberOfOpenPairs = 0;  // количество раскрытых пар одинаковых по номиналу и масти карт

function changeCards() {
    // переворачиваем карты рубашками вверх
    for (var i = 0; i < countOfPlaces; i++) {
        $("#c" + i + " img").attr('src', './images/shirtCard.png');
        $("#c" + i + " img").attr('data-tid', 'Card-flipped');
    }

    // включаем функцию клика по картам
    $('.card img').on('click', clickOnCard);

    function clickOnCard() {

        // выключаем функцию клика по картам
        $('.card img').off('click', clickOnCard);

        // ищем порядковый номер места, на котором была щелкнута карта;
        // номер места может принимать значения от 0 до 17 включительно
        for (var i = 0; i < countOfPlaces; i++) {
            if (indexOfSelectedPlaces[i] === parseInt($(this).attr("id"))) {
                index = i;
                break;
            }
        }

        // открываем щелкнутую карту (переворачиваем рубашкой вниз)
        $(this).attr('src', arrayOfImages[indexOfSelectedCards[index]]);
        $(this).attr('data-tid', 'Card');
        
        if (flag == true) { //если это была первая открытая карта из двух карт
            indexPrev = index;
            flag = false;
            $('.card img').on('click', clickOnCard); // включаем функцию клика по картам
        }
        else {
            if (index != indexPrev) { //если были нажаты две разные карты, а не одна и та же
                setTimeout(function () {
                    //если две открытые карты совпали
                    if (arrayOfImages[indexOfSelectedCards[indexPrev]] == arrayOfImages[indexOfSelectedCards[index]]) {
                        numberOfOpenPairs++;
                        scoreValue += (9 - numberOfOpenPairs) * 42;
                        $("#c" + indexOfSelectedPlaces[indexPrev] + " img," + "#c" + indexOfSelectedPlaces[index] + " img").fadeTo(200, 0, changeVisibility);
                        playAudio('./audio/right.wav');
                    }
                    else {
                        scoreValue -= (numberOfOpenPairs) * 42;

                        $("#c" + indexOfSelectedPlaces[indexPrev] + " img").attr('src', './images/shirtCard.png');
                        $("#c" + indexOfSelectedPlaces[index] + " img").attr('src', './images/shirtCard.png');

                        $("#c" + indexOfSelectedPlaces[indexPrev] + " img").attr('data-tid', 'Card-flipped');
                        $("#c" + indexOfSelectedPlaces[index] + " img").attr('data-tid', 'Card-flipped');

                        playAudio('./audio/wrong.wav');
                        prepareToNextTurn();
                    }
                }, 400);
            }
            else {
                $('.card img').on('click', clickOnCard);
            }
        }
    }

    // функция, выполняющая изменения значения свойства visibility двух открытых карт при их совпадении
    function changeVisibility() {
        $("#c" + indexOfSelectedPlaces[indexPrev] + " img").css('visibility', 'hidden');
        $("#c" + indexOfSelectedPlaces[index] + " img").css('visibility', 'hidden');
        prepareToNextTurn(); 
    }

    // функция "подготовки" к следующему ходу игрока
    function prepareToNextTurn() {
        $('.scoreValue').text(scoreValue); // обновить счет
        $('.card img').on('click', clickOnCard); // активировать функцию клика по картам
        flag = true;
        if (numberOfOpenPairs == 9) {
            setTimeout(function () {
                window.location = "endPage.html?value=" + scoreValue;
            },600);
        }
    }

    // функция, отвечающая за воспроизведение звуков
    function playAudio(path) {
        var myAudio = new Audio;
        myAudio.src = path;
        myAudio.play();
    }
}

