<?php
  $date = getdate();
  $nameFile = $date["mon"]."_".$date["year"].$_POST["machine"].".csv";
  $fp = fopen("log/".$nameFile,"a");
  fwrite($fp,$_POST["machine"].";".$_POST["log"]."\n");  
  fclose($fp);
?>