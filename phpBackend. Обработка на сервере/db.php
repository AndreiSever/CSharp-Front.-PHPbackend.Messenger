<?	
	
	$config = array(
		// FRAMEWORK_CONFIGURATION
		'framework' => array (
			'development' => 'false'
		),
	
		// PATH_CONFIGURATION
		'path' => array (
			'languages' => 'ru',
			'template' => 'practical'
		),
		
		// MYSQL_CONFIGURATION
		'mysql' => array (
			'host' => 'localhost',
			'db' => 'host1379770_music',
			'user' => 'host1379770_andr',
			'pass' => 'andrei',
			'charset' => 'utf8'
		),
	);

	$db = mysql_connect($config['mysql']['host'], $config['mysql']['user'], $config['mysql']['pass']);
	mysql_select_db ($config['mysql']['db'], $db);
	mysql_set_charset($config['mysql']['charset']);
?>	