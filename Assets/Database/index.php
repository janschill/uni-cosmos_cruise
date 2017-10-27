<?php
  $servername = "localhost";
  $username = "root";
  $password = "50cent50cent";
  $dbname = "cosmoscruise";

  $conn = new mysqli($servername, $username, $password, $dbname);

  if (!$conn)
  {
    die("Connection failed. ". mysqli_connect_error());
  }

  $sql = "SELECT id, name, level, time, totaltime FROM Player";
  $result = mysqli_query($conn, $sql);

  if(mysqli_num_rows($result) > 0)
  {
    while ($row = mysqli_fetch_assoc($result))
    {
      echo "id".$row['id']."|name".$row['name']."|level".$row['level']."|time".$row['time']."|totaltime".$row['totaltime'].";";
    }
  }
 ?>
