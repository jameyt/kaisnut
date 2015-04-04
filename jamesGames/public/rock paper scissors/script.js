$(document).ready(function(){
	hide();

	$('#rock').click(function(e){
		hide();	
		rps(1);
	})
	$('#paper').click(function(e){
		hide();
		rps(2);
	})
	$('#scissors').click(function(e){
		hide();
		rps(3);
	})
	
function rps(userChoice)
{
	var computerChoice = Math.floor(Math.random()*(3)+1);
	showUserChoice(userChoice);
	showComputerChoice(computerChoice);

	if(userChoice===computerChoice){
		tie();
	}else if (userChoice === 1) {
		if(computerChoice === 3){
			win();
		}else{
			lose();
		}
	} else if (userChoice === 2) {
		if(computerChoice === 1){
			win();
		}else{
			lose();
		}
	} else{
		if(computerChoice === 2){
			win();
		}else{
			lose();
		}
	};
}

function win()
{
	$('#win').show("fast");
	$('#result').html('you win!');
}

function lose()
{
	$('#lose').show("fast");
	$('#result').html('you lose!');
}

function tie()
{
	$('#tie').show("fast");
	$('#result').html('you tie!');
}

function showUserChoice(userChoice)
{	
	if (userChoice === 1){
		$('#userChoice').html('you chose rock');
	} else if(userChoice === 2){		
		$('#userChoice').html('you chose paper');
	} else if(userChoice === 3){		
		$('#userChoice').html('you chose scissors');
	}
}

function showComputerChoice(computerChoice)
{
	if (computerChoice === 1){
		$('#computerRock').show("fast");
		$('#computerChoice').html('computer chose rock');
	} else if(computerChoice === 2){
		$('#computerPaper').show("fast");
		$('#computerChoice').html('computer chose paper');
	} else if(computerChoice === 3){
		$('#computerScissors').show("fast");
		$('#computerChoice').html('computer chose scissors');
	}
}

function hide()
{
	$('#computerRock').hide("fast");
	$('#computerPaper').hide("fast");
	$('#computerScissors').hide("fast");
	$('#win').hide("fast");
	$('#lose').hide("fast");
	$('#tie').hide("fast");
}

});