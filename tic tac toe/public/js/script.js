$(document).ready(function(){
	
	var endGame = false;
	var marked = ['','','','','','','','',''];
	var difficulty = 5;
	
	$('#1').click(function(e){
		gameRound(1);
	})
	$('#2').click(function(e){
		gameRound(2);
	})
	$('#3').click(function(e){
		gameRound(3);
	})
	$('#4').click(function(e){
		gameRound(4);
	})
	$('#5').click(function(e){
		gameRound(5);
	})
	$('#6').click(function(e){
		gameRound(6);
	})
	$('#7').click(function(e){
		gameRound(7);
	})
	$('#8').click(function(e){
		gameRound(8);
	})
	$('#9').click(function(e){
		gameRound(9);
	})
	$('#changeDifficulty').click(function(e){
		chooseDifficulty();
	})

	function gameRound(userChoice){		
		if(!endGame){
			var userChoiceMarked = markUser(userChoice);
			if(userChoiceMarked){
				endGame = checkGameEnd();
				if(!endGame){computerChoice();
					endGame = checkGameEnd();
				}
			}
		}
	}

	function computerChoice(){
		var computerNum = 0;
		var computerMarked = false;
		var allMarked = checkAllMarked();
				
		while(!computerMarked && !allMarked){
			computerNum = computerIntelligentSelection();			
			computerMarked = markComputer(computerNum);				
		}
	}

	function computerIntelligentSelection(){
		var computerChoice = -1;
		//block user from winning
		if(marked[0] == 'o' && marked[1] == 'o' && marked[2] == ''){computerChoice = 2;}
		if(marked[3] == 'o' && marked[4] == 'o' && marked[5] == ''){computerChoice = 5;}
		if(marked[6] == 'o' && marked[7] == 'o' && marked[8] == ''){computerChoice = 8;}

		if(marked[0] == 'o' && marked[3] == 'o' && marked[6] == ''){computerChoice = 6;}
		if(marked[1] == 'o' && marked[4] == 'o' && marked[7] == ''){computerChoice = 7;}
		if(marked[2] == 'o' && marked[5] == 'o' && marked[8] == ''){computerChoice = 8;}

		if(marked[0] == 'o' && marked[4] == 'o' && marked[8] == ''){computerChoice = 8;}
		if(marked[2] == 'o' && marked[4] == 'o' && marked[6] == ''){computerChoice = 6;}

		if(marked[0] == 'o' && marked[2] == 'o' && marked[1] == ''){computerChoice = 1;}
		if(marked[3] == 'o' && marked[5] == 'o' && marked[4] == ''){computerChoice = 4;}
		if(marked[6] == 'o' && marked[8] == 'o' && marked[7] == ''){computerChoice = 7;}

		if(marked[0] == 'o' && marked[6] == 'o' && marked[3] == ''){computerChoice = 3;}
		if(marked[1] == 'o' && marked[7] == 'o' && marked[4] == ''){computerChoice = 4;}
		if(marked[2] == 'o' && marked[8] == 'o' && marked[5] == ''){computerChoice = 5;}

		if(marked[0] == 'o' && marked[8] == 'o' && marked[4] == ''){computerChoice = 4;}
		if(marked[2] == 'o' && marked[6] == 'o' && marked[4] == ''){computerChoice = 4;}

		if(marked[1] == 'o' && marked[2] == 'o' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'o' && marked[5] == 'o' && marked[3] == ''){computerChoice = 3;}
		if(marked[7] == 'o' && marked[8] == 'o' && marked[6] == ''){computerChoice = 6;}

		if(marked[3] == 'o' && marked[6] == 'o' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'o' && marked[7] == 'o' && marked[1] == ''){computerChoice = 1;}
		if(marked[5] == 'o' && marked[8] == 'o' && marked[2] == ''){computerChoice = 2;}

		if(marked[4] == 'o' && marked[8] == 'o' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'o' && marked[6] == 'o' && marked[2] == ''){computerChoice = 2;}

		//try to win
		if(marked[0] == 'x' && marked[1] == 'x' && marked[2] == ''){computerChoice = 2;}
		if(marked[3] == 'x' && marked[4] == 'x' && marked[5] == ''){computerChoice = 5;}
		if(marked[6] == 'x' && marked[7] == 'x' && marked[8] == ''){computerChoice = 8;}

		if(marked[0] == 'x' && marked[3] == 'x' && marked[6] == ''){computerChoice = 6;}
		if(marked[1] == 'x' && marked[4] == 'x' && marked[7] == ''){computerChoice = 7;}
		if(marked[2] == 'x' && marked[5] == 'x' && marked[8] == ''){computerChoice = 8;}

		if(marked[0] == 'x' && marked[4] == 'x' && marked[8] == ''){computerChoice = 8;}
		if(marked[2] == 'x' && marked[4] == 'x' && marked[6] == ''){computerChoice = 6;}

		if(marked[0] == 'x' && marked[2] == 'x' && marked[1] == ''){computerChoice = 1;}
		if(marked[3] == 'x' && marked[5] == 'x' && marked[4] == ''){computerChoice = 4;}
		if(marked[6] == 'x' && marked[8] == 'x' && marked[7] == ''){computerChoice = 7;}

		if(marked[0] == 'x' && marked[6] == 'x' && marked[3] == ''){computerChoice = 3;}
		if(marked[1] == 'x' && marked[7] == 'x' && marked[4] == ''){computerChoice = 4;}
		if(marked[2] == 'x' && marked[8] == 'x' && marked[5] == ''){computerChoice = 5;}

		if(marked[0] == 'x' && marked[8] == 'x' && marked[4] == ''){computerChoice = 4;}
		if(marked[2] == 'x' && marked[6] == 'x' && marked[4] == ''){computerChoice = 4;}

		if(marked[1] == 'x' && marked[2] == 'x' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'x' && marked[5] == 'x' && marked[3] == ''){computerChoice = 3;}
		if(marked[7] == 'x' && marked[8] == 'x' && marked[6] == ''){computerChoice = 6;}

		if(marked[3] == 'x' && marked[6] == 'x' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'x' && marked[7] == 'x' && marked[1] == ''){computerChoice = 1;}
		if(marked[5] == 'x' && marked[8] == 'x' && marked[2] == ''){computerChoice = 2;}

		if(marked[4] == 'x' && marked[8] == 'x' && marked[0] == ''){computerChoice = 0;}
		if(marked[4] == 'x' && marked[6] == 'x' && marked[2] == ''){computerChoice = 2;}

		
		var randomDifficulty = random5() + Number(difficulty);		
		computerChoice = computerChoice+1;		
		if(computerChoice == 0 || randomDifficulty < 5) {computerChoice = random9();}		
		return computerChoice;
	}

	function checkGameEnd(){
		
		var userVictory = checkUserVictory();
		var computerVictory = checkComputerVictory();
		var draw = checkAllMarked();

		if(userVictory){
			$('.endGame').html('you win!');			
		}else if(computerVictory){
			$('.endGame').html('you lose!');			
		}else if(draw){
			$('.endGame').html('draw');}

		return userVictory || computerVictory || draw;
	}

	function checkUserVictory(){
		if(marked[0] == 'o' && marked[1] == 'o' && marked[2] == 'o'){return true;}
		if(marked[3] == 'o' && marked[4] == 'o' && marked[5] == 'o'){return true;}
		if(marked[6] == 'o' && marked[7] == 'o' && marked[8] == 'o'){return true;}

		if(marked[0] == 'o' && marked[3] == 'o' && marked[6] == 'o'){return true;}
		if(marked[1] == 'o' && marked[4] == 'o' && marked[7] == 'o'){return true;}
		if(marked[2] == 'o' && marked[5] == 'o' && marked[8] == 'o'){return true;}

		if(marked[0] == 'o' && marked[4] == 'o' && marked[8] == 'o'){return true;}
		if(marked[2] == 'o' && marked[4] == 'o' && marked[6] == 'o'){return true;}

		return false;
	}

	function checkComputerVictory(){
		if(marked[0] == 'x' && marked[1] == 'x' && marked[2] == 'x'){return true;}
		if(marked[3] == 'x' && marked[4] == 'x' && marked[5] == 'x'){return true;}
		if(marked[6] == 'x' && marked[7] == 'x' && marked[8] == 'x'){return true;}

		if(marked[0] == 'x' && marked[3] == 'x' && marked[6] == 'x'){return true;}
		if(marked[1] == 'x' && marked[4] == 'x' && marked[7] == 'x'){return true;}
		if(marked[2] == 'x' && marked[5] == 'x' && marked[8] == 'x'){return true;}

		if(marked[0] == 'x' && marked[4] == 'x' && marked[8] == 'x'){return true;}
		if(marked[2] == 'x' && marked[4] == 'x' && marked[6] == 'x'){return true;}

		return false;
	}

	function markUser(num){		
		if(!checkMark(num)){

			mark(num,chooseO());
			marked[num-1] = 'o';

			return true;
		}else{
			alert('position already taken!');
			return false;
		}
	}

	function checkMark(num){
		return marked[num-1];
	}

	function checkAllMarked(){
		for (var i = 0; i < marked.length; i++)
		{
			if(marked[i] == ''){	return false; }
		}
		return true;
	}

	function markComputer(num){		
		if(!checkMark(num)){
			mark(num,chooseX());
			marked[num-1] = 'x';
			return true;
		}		
		return false;
	}

	function mark(num, fileName){
		var id = "#" + num;
		var htmlString = "<a id='" + num + "' href='#'><img src='" + fileName + "' alt='" + num + "'></a>";
		$(id).html(htmlString);
	}

	function chooseO()
	{
		var num = random3();
		var o = "";

		switch(num){
			case 1:
				o = "blue-o.png";
				break;
			case 2:
				o = "osu-o.png";
				break;
			case 3:
				o = "red-o.png";
				break;
			default:
				alert("error: chooseO");
		}

		return o;
	}

	function chooseX()
	{
		var num = random3();
		var x = "";

		switch(num){
			case 1:
				x = "blue-x.png";
				break;
			case 2:
				x = "green-x.png";
				break;
			case 3:
				x = "red-x.png";
				break;
			default:
				alert("error: chooseX");
		}

		return x;
	}

	function random3(){
		return randomNumber(0,3);
	}

	function random9(){
		return randomNumber(0,9);
	}

	function random5(){
		return randomNumber(0,5);
	}

	function randomNumber(min,max){
		return Math.ceil(Math.random()*(max-min)+min);
	}

	function chooseDifficulty(){
		difficulty = 0;
		while (difficulty<1 || difficulty >5){
			difficulty = prompt('choose your difficulty (1-5)');
		}
	}

});