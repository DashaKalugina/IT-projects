var generationTime = 500;
var cnv=document.querySelector('canvas');
cnv.width=600;
cnv.height=600;
var numberCell=30;
var ctx = cnv.getContext('2d');
drawGrid(numberCell);
var sizeCell=cnv.width/numberCell;
var startingGeneration=new Array(numberCell);
generateNewArray(startingGeneration,numberCell);
var flagWasFilled = false;
var numberGeneration=1;
var interval=0;
cnv.onclick = getCursorPosition;
var buttonStart=document.getElementById("start");
buttonStart.onclick = Start;

function drawGrid(numberCell){
    var w=cnv.width;
    var h=cnv.height;
    var sizeCell=w/numberCell;
    for(var x=0;x<w;x+=sizeCell){
        for(var y=0;y<h;y+=sizeCell)
            ctx.strokeRect(x,y,sizeCell,sizeCell);
    }
}

function generateNewArray(array,size){
    for(var i=0;i<size;i++){
        array[i]=new Array(size);
        array[i].fill(0);
    } 
}

function getCursorPosition(event) {
    var rect = this.getBoundingClientRect();
    var x = event.clientX - rect.left;
    var y = event.clientY - rect.top;
    var w=this.width;
    var yCell=Number(Math.floor(x/sizeCell));
    var xCell=Number(Math.floor(y/sizeCell));

    if (startingGeneration[xCell][yCell]==1)
    {
        ctx.clearRect(yCell*sizeCell,xCell*sizeCell,sizeCell,sizeCell);
        ctx.strokeRect(yCell*sizeCell,xCell*sizeCell,sizeCell,sizeCell);
        startingGeneration[xCell][yCell]=0;
    }
    else
    {
        startingGeneration[xCell][yCell]=1;
        ctx.fillStyle = "green";
        ctx.fillRect(yCell*sizeCell,xCell*sizeCell,sizeCell,sizeCell);
    }
    flagWasFilled=true;
}

function increaseCountOfGenerations(){
        numberGeneration++;
        var el=document.getElementById("numberCurrentGeneration");
        el.textContent="Number of generations: "+numberGeneration;
}

function ClearAllField(){
    for(var i=0;i<startingGeneration.length;i++){
        for(var j=0;j<startingGeneration.length;j++){
            if (startingGeneration[i][j]==1){
                ClearOneCell(i,j);
            }
        }
    }
}

function ClearOneCell(i,j){
    ctx.clearRect(j*sizeCell,i*sizeCell,sizeCell,sizeCell);
    ctx.strokeRect(j*sizeCell,i*sizeCell,sizeCell,sizeCell);
}

function SendTextByID(id, text){
    var e1=document.getElementById(id);
    e1.textContent=text;
}

function Restart(event){
    clearInterval(interval);
    ClearAllField();
    SendTextByID("numberCurrentGeneration","Number of generations: 0");
    SendTextByID("start","Start");
    buttonStart.onclick = Start;
    cnv.onclick = getCursorPosition;
    generateNewArray(startingGeneration,numberCell);
    numberGeneration=1;
    flagWasFilled=false;
}

function Start(event)
{   
    if (flagWasFilled)
        SendTextByID("numberCurrentGeneration","Number of generations: 1")        
    SendTextByID("start","Restart")
    buttonStart.onclick = Restart;
    cnv.onclick=null;
    interval = setInterval(function(){
        var flagGeneration=false;
        var sizeArray=startingGeneration.length;
        var newGeneration=new Array(sizeArray);
        generateNewArray(newGeneration,sizeArray);
        for(var i=0;i<sizeArray;i++){
            for(var j=0;j<sizeArray;j++){
                var countNeighbors=0;
                if(j==0 && i==0){
                    //[0][0]ая ячейка
                    countNeighbors=checkNeighbors(countNeighbors,[i+1,i,i+1],[j,j+1,j+1]);
                    flagGeneration=changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(i==0 && j==sizeArray-1){
                    //[0][array.length-1]ая ячейка
                    countNeighbors=checkNeighbors(countNeighbors,[i+1,i,i+1],[j,j-1,j-1]);
                    flagGeneration= changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(i==0){
                    //[0][все остальные]
                    countNeighbors=checkNeighbors(countNeighbors,[i,i,i+1,i+1,i+1],[j-1,j+1,j-1,j,j+1]);
                    flagGeneration=  changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(j==0 && i==sizeArray-1){
                    //[array.length-1][0]ая ячейка
                    countNeighbors=checkNeighbors(countNeighbors,[i-1,i,i-1],[j,j+1,j+1]);
                    flagGeneration= changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(j==0){
                    //[все остальные][0]
                    countNeighbors=checkNeighbors(countNeighbors,[i-1,i-1,i,i+1,i+1],[j,j+1,j+1,j+1,j]);
                   flagGeneration= changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(i==sizeArray-1 && j==sizeArray-1){
                    //[array.length-1][array.length-1]ая ячейка
                    countNeighbors=checkNeighbors(countNeighbors,[i,i-1,i-1],[j-1,j-1,j]);
                    flagGeneration=  changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(i==sizeArray-1){
                    //[array.length-1][все остальные]
                    countNeighbors=checkNeighbors(countNeighbors,[i,i-1,i-1,i-1,i],[j-1,j-1,j,j+1,j+1]);
                    flagGeneration= changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else if(j==sizeArray-1){
                    //[все остальные][array.length-1]
                    countNeighbors=checkNeighbors(countNeighbors,[i+1,i+1,i,i-1,i-1],[j,j-1,j-1,j-1,j]);
                    flagGeneration=  changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
                else{
                    //центральная
                    countNeighbors=checkNeighbors(countNeighbors,[i-1,i,i+1,i+1,i+1,i,i-1,i-1],[j+1,j+1,j+1,j,j-1,j-1,j-1,j]);
                    flagGeneration= changeStatus(newGeneration,countNeighbors,flagGeneration,i,j);
                    continue;
                }
            }
        }
        if(flagGeneration)
            increaseCountOfGenerations();
        swapArrays(newGeneration);
    },generationTime);
}

function checkNeighbors(countNeighbors,arrayI,arrayJ){
    var count=countNeighbors;
    for(var k=0;k<arrayI.length;k++){
        var indexI=arrayI[k];
        var indexJ=arrayJ[k]
        if(startingGeneration[indexI][indexJ]==1)
        count++; 
    }
return count;
}

function changeStatus(newGeneration,countNeighbors,flagGeneration,i,j){

    if(startingGeneration[i][j]==1){
        //живая
        if(countNeighbors<2 || countNeighbors>3){          
             newGeneration[i][j]=0;//умирает
             flagGeneration=true;
             ClearOneCell(i,j);
        }
        else{
            newGeneration[i][j]=1;//остается жить
            ctx.fillRect(j*sizeCell,i*sizeCell,sizeCell,sizeCell);
        }
    }
    else{
        //Никого еще нет
        if(countNeighbors==3){
            newGeneration[i][j]=1;//рождается
            flagGeneration=true;
            ctx.fillRect(j*sizeCell,i*sizeCell,sizeCell,sizeCell);
        }
    }
    return flagGeneration;
}

function swapArrays(newGeneration){
    for(var i=0;i<newGeneration.length;i++){
        for(var j=0;j<newGeneration.length;j++){
            startingGeneration[i][j]=newGeneration[i][j];
        }
    }
}