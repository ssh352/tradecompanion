<?php
/** 
*
* @package acp
* @version $Id: acp_ranks.php,v 1.2 2006/05/01 19:45:42 grahamje Exp $
* @copyright (c) 2005 phpBB Group 
* @license http://opensource.org/licenses/gpl-license.php GNU Public License 
*
*/

/**
* @package module_install
*/
class acp_ranks_info
{
	function module()
	{
		return array(
			'filename'	=> 'acp_ranks',
			'title'		=> 'ACP_RANKS',
			'version'	=> '1.0.0',
			'modes'		=> array(
				'ranks'		=> array('title' => 'ACP_MANAGE_RANKS', 'auth' => 'acl_a_ranks', 'cat' => array('ACP_CAT_USERS')),
			),
		);
	}

	function install()
	{
	}

	function uninstall()
	{
	}
}

?>