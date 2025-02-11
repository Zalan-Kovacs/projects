<?php
session_start();
$error = "";

if ($_POST) {
    $email = $_POST["email"] ?? "";
    $password = $_POST["password"] ?? "";

    $users = json_decode(file_get_contents("users.json"), true);

    foreach ($users as $index => $user) {
        if ($user["email"] === $email && password_verify($password, $user["password"])) {
            $_SESSION["user_id"] = $index;
            header("location: index.php");
            exit();
        }
    }
    $error = "Hibás e-mail cím vagy jelszó.";
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>Bejelentkezés</title>
</head>
<body>
    <ul>
        <li><a>iKarRental</a></li>
        <li class="link"><a href="register.php">Regisztráció</a></li>
        <li class="link"><a href="index.php">Főoldal</a></li>
    </ul>
    <?php
        if($_POST && $failed)
        {
            echo "Failed";
        }

    ?>


    <form method="post" action="login.php">
        E-mail: <input type="text" name="email"><br>
        Jelszó: <input type="password" name="password"><br>
        <?= $error ?>
        <button type="submit"> Bejelentkezés</button>
    </form>
</body>
</html>