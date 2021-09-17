<?
	require('db.php');
	//$email= myxor($_GET['email'], $key);
	$email=base64_decode($_GET['email']);
	//$email=$_GET['email'] ^ $key;
	//echo $email;
	$pubemail=base64_decode($_GET['pubemail']);
	//echo $qw= myxor(12, 123);
	$password=base64_decode($_GET['password']);
	//echo $password;
	//$password=$_GET['password'] ^ $key;
	$action=$_GET['action'];
	$message=base64_decode($_GET['message']);
	$id=$_GET['id'];
	$sobesednik=$_GET['sobesednik'];
	$firstname=$_GET['firstname'];
	$secondname=$_GET['secondname'];
	$thirdname=$_GET['thirdname'];
	$sobesednik1=explode(' ', $sobesednik);
	
	function calculate_time_difference($timestampl, $timestamp2, $time_unit)  { 
// Определяем разницу между двумя датами 
	$timestampl = intval($timestampl); 
	$timestamp2 = intval($timestamp2); 
	if ($timestampl && $timestamp2)  {
	$time_lapse = $timestamp2 - $timestampl;

	$seconds_in_unit = array(
	"second" => 1,
	"minute" => 60,
	"hour" => 3600,
	"day" => 86400,
	"week" => 604800,
	);

if ($seconds_in_unit[$time_unit])  {
return floor($time_lapse/$seconds_in_unit[$time_unit]);
}
}
return false; 
}
	
	
	if ($action=='login') {
		$result=mysql_query("SELECT * FROM fs_users WHERE email='".$email."' AND password='".$password."' AND `fs_users`.`group`<>'-1'");
		$myrow=mysql_fetch_array($result);
	if (!empty($myrow['id'])){
		if ($myrow['group']==1){
		echo '<request>
			  <login>Admin</login>
			  <id>'.base64_encode ($myrow['id']).'</id>
			  <firstname>'.base64_encode ($myrow['name']).'</firstname>
			  <secondname>'.base64_encode ($myrow['secondname']).'</secondname>
			  <thirdname>'.base64_encode ($myrow['thirdname']).'</thirdname>
			  </request>';
		}
		else {
			echo '<request>
			  <login>true</login>
			 <id>'.base64_encode ($myrow['id']).'</id>
			  <firstname>'.base64_encode ($myrow['name']).'</firstname>
			  <secondname>'.base64_encode ($myrow['secondname']).'</secondname>
			  <thirdname>'.base64_encode ($myrow['thirdname']).'</thirdname>
			  </request>';
		}
		
	}
	 else {
		echo '<request>
			  <login>false</login>
			  </request>';
	} 
	}
	if ($action=='find') {
		$result=mysql_query("SELECT * FROM fs_users WHERE id='".$id."' AND `fs_users`.`group`='1'")or die(mysql_error());
		$myrow=mysql_fetch_array($result);
	if (!empty($myrow['id'])){
		$result1=mysql_query("SELECT * FROM fs_users WHERE name='".$sobesednik1[0]."' AND secondname='".$sobesednik1[1]."' AND thirdname='".$sobesednik1[2]."'")or die(mysql_error());
		$myrow1=mysql_fetch_array($result1);
					echo '<request>
				  <firstname>'.$myrow1['name'].'</firstname>
				  <secondname>'.$myrow1['secondname'].'</secondname>
				  <thirdname>'.$myrow1['thirdname'].'</thirdname>
				  <email>'.base64_encode($myrow1['email']).'</email>
				  <password>'.base64_encode($myrow1['password']).'</password>
				  </request>';

		
	}
	}
	if ($action=='listboxAdm') {
		$result=mysql_query("SELECT * FROM fs_users WHERE id<>'".$id."' AND `fs_users`.`group`<>'-1'");
		while ($myrow=mysql_fetch_array($result)){
					echo '<request>
				  <firstname>'.$myrow['name'].'</firstname>
				  <secondname>'.$myrow['secondname'].'</secondname>
				  <thirdname>'.$myrow['thirdname'].'</thirdname>
				  </request>';

		}
	}
		if ($action=='insert'){
		$result=mysql_query("SELECT * FROM fs_users WHERE id='".$id."' AND `fs_users`.`group`='1'")or die(mysql_error());
			$myrow=mysql_fetch_array($result);
			if (!empty($myrow['id'])){
				$result1=mysql_query("SELECT * FROM fs_users WHERE name='".$firstname."' AND secondname='".$secondname."' AND thirdname='".$thirdname."'")or die(mysql_error());
				$myrow1=mysql_fetch_array($result1);
				if (empty($myrow1['id'])){
					$result2=mysql_query("SELECT * FROM fs_users WHERE email='".$email."'")or die(mysql_error());
					$myrow2=mysql_fetch_array($result2);
					if (empty($myrow2['id'])){
						$post = mysql_query("INSERT INTO `host1379770_music`.`fs_users` (`name`, `secondname`, `thirdname`,`email`, `password`, `group`) VALUES ('$firstname', '$secondname','$thirdname', '$email', '$password', '0')");
					}
					else{
						echo '<request>
							  <login>email</login>
			                  </request>';
					}
				}
				else {
					echo '<request>
						  <login>name</login>
			              </request>';
				}
		}
	}	
	
	if ($action=='update'){
			$result=mysql_query("SELECT * FROM fs_users WHERE id='".$id."' AND `fs_users`.`group`='1'")or die(mysql_error());
			$myrow=mysql_fetch_array($result);
			if (!empty($myrow['id'])){
				$result1=mysql_query("SELECT * FROM fs_users WHERE name='".$firstname."' AND secondname='".$secondname."' AND thirdname='".$thirdname."' AND email<>'".$pubemail."'")or die(mysql_error());
				$myrow1=mysql_fetch_array($result1);
				if (empty($myrow1['id'])){
					$result2=mysql_query("SELECT * FROM fs_users WHERE email='".$email."' AND email<>'".$pubemail."'")or die(mysql_error());
					$myrow2=mysql_fetch_array($result2);
					if (empty($myrow2['id'])){
						$result3=mysql_query("SELECT * FROM fs_users WHERE email='".$pubemail."'")or die(mysql_error());
						$myrow3=mysql_fetch_array($result3);
						if (!empty($myrow3['id'])){
							$post = mysql_query("UPDATE `host1379770_music`.`fs_users` SET name = '$firstname', `secondname` = '$secondname',`thirdname` = '$thirdname', email = '$email', password = '$password' WHERE `fs_users`.`id` = '".$myrow3['id']."'");
						}
					}
					else{
						echo '<request>
							  <login>email</login>
			                  </request>';
					}
				}
				else {
					echo '<request>
						  <login>name</login>
			              </request>';
				}
		
				}
				}
		
	
		if ($action=='delete'){
			$result=mysql_query("SELECT * FROM fs_users WHERE id='".$id."' AND `fs_users`.`group`='1'")or die(mysql_error());
			$myrow=mysql_fetch_array($result);
			if (!empty($myrow['id'])){
				$post = mysql_query("UPDATE `host1379770_music`.`fs_users` SET `group` = '-1' WHERE `fs_users`.`email` = '".$email."'")or die(mysql_error());
		}
	}	
	
	if ($action=='listbox') {
		$result=mysql_query("SELECT * FROM fs_users WHERE id<>'".$id."' AND `fs_users`.`group`<>'-1'");
		while ($myrow=mysql_fetch_array($result)){
			$flag=0;
			$result1=mysql_query("SELECT * FROM fs_online WHERE id='".$myrow['id']."'");
			$myrow1=mysql_fetch_array($result1);
			if (!empty($myrow1['id'])){
				 $result3=mysql_query("SELECT * FROM fs_newmess WHERE uid='".$id."' AND tid='".$myrow['id']."'");
				$myrow3=mysql_fetch_array($result3); 
				
				$timestamp1 = strtotime($myrow1['date']);
				$timestamp2 = time();
				
				$time = calculate_time_difference($timestamp1, $timestamp2, "second");
				$result2=mysql_query("SELECT * FROM fs_messages WHERE (uid='".$myrow['id']."' AND tid='".$id."')");
				while ($myrow2=mysql_fetch_array($result2)){  
				if ($myrow2['date']>$myrow3['date']) {
					$flag=1;
				}
				}
				if ($flag==1){
				if ($time < 60) {
				echo '<request>
				  <login>Online</login>
				  <firstname>'.$myrow['name'].'</firstname>
				  <secondname>'.$myrow['secondname'].'</secondname>
				  <thirdname>'.$myrow['thirdname'].'</thirdname>
				  <message>*Новое сообщение</message>
				  </request>';
				}
				else {
					echo '<request>
				  <login>Offline</login>
				  <firstname>'.$myrow['name'].'</firstname>
				  <secondname>'.$myrow['secondname'].'</secondname>
				  <thirdname>'.$myrow['thirdname'].'</thirdname>
				  <message>*Новое сообщение</message>
				  </request>';
				
				}
				}
				else { 
					if ($time < 60) {
				echo '<request>
				  <login>Online</login>
				  <firstname>'.$myrow['name'].'</firstname>
				  <secondname>'.$myrow['secondname'].'</secondname>
				  <thirdname>'.$myrow['thirdname'].'</thirdname>
				<message> </message>
				  </request>';
				}
				else {
					echo '<request>
				  <login>Offline</login>
				  <firstname>'.$myrow['name'].'</firstname>
				  <secondname>'.$myrow['secondname'].'</secondname>
				  <thirdname>'.$myrow['thirdname'].'</thirdname>
				<message> </message>
				  </request>';
				
				}
				}
			}
		}
	}
	if ($action=='message') {
		
		$result=mysql_query("SELECT * FROM fs_users WHERE name='".$sobesednik1[1]."' AND secondname='".$sobesednik1[2]."' AND thirdname='".$sobesednik1[3]."'");
		$myrow=mysql_fetch_array($result);
		
		 $result2=mysql_query("SELECT * FROM fs_newmess WHERE uid='".$id."' AND tid='".$myrow['id']."'");
		$myrow2=mysql_fetch_array($result2);
		if (!empty($myrow2['uid'])){
			$post = mysql_query("UPDATE fs_newmess SET date=NOW() WHERE uid='".$id."' AND tid='".$myrow['id']."'");
		}
		else {
		$post = mysql_query("INSERT INTO fs_newmess(uid, tid, date) VALUES('$id','".$myrow['id']."', NOW())");
		}	 
		
		$result1=mysql_query("SELECT * FROM fs_messages WHERE (uid='".$id."' AND tid='".$myrow['id']."') OR (uid='".$myrow['id']."' AND tid='".$id."')");
		while ($myrow1=mysql_fetch_array($result1)){
			if ($myrow1['uid']==$id){
			echo '<request>
			  <firstname>'.$firstname.'</firstname>
			  <secondname>'.$secondname.'</secondname>
			  <thirdname>'.$thirdname.'</thirdname>
			  <message>'.base64_encode($myrow1['message']).'</message>
			  <date>'.$myrow1['date'].'</date>
			  </request>';
			  
			}
			else {
				echo '<request>
			  <firstname>'.$sobesednik1[1].'</firstname>
			  <secondname>'.$sobesednik1[2].'</secondname>
			  <thirdname>'.$sobesednik1[3].'</thirdname>
			  <message>'.base64_encode($myrow1['message']).'</message>
			  <date>'.$myrow1['date'].'</date>
			  </request>';
			}
		}
	}
	if ($action=='send') {
		$result=mysql_query("SELECT * FROM fs_users WHERE name='".$sobesednik1[1]."' AND secondname='".$sobesednik1[2]."' AND thirdname='".$sobesednik1[3]."'");
		$myrow=mysql_fetch_array($result);
		$post = mysql_query("INSERT INTO fs_messages(uid, tid, message, file, date) VALUES('$id','".$myrow['id']."','$message','$file', NOW())");
	}
	if ($action=='online'){
		$result=mysql_query("SELECT * FROM fs_online WHERE id='".$id."'");
		$myrow=mysql_fetch_array($result);
		if (!empty($myrow['id'])){
			$post = mysql_query("UPDATE fs_online SET date=NOW() WHERE id='".$id."'");
		}
		else {
		$post = mysql_query("INSERT INTO fs_online(id, date) VALUES('$id', NOW())");
		}
	}	
		
	
?>