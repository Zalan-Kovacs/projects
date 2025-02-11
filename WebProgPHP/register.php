<?php
session_start();
$errors = [];

if ($_POST) {
    $name = trim($_POST["name"] ?? "");
    $email = trim($_POST["email"] ?? "");
    $password = $_POST["password"] ?? "";
    $confirm_password = $_POST["confirm_password"] ?? "";

    if (!$name) {
        $errors['name'] = "A név megadása kötelező.";
    } else if(strlen($name) < 2){
        $errors['name'] = "Érvénytelen név.";
    }

    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        $errors['email'] = "Érvénytelen e-mail cím.";
    }

    if (strlen($password) < 6) {
        $errors['password'] = "A jelszónak legalább 6 karakter hosszúnak kell lennie.";
    }

    if ($password !== $confirm_password) {
        $errors['confirm'] = "A jelszavak nem egyeznek.";
    }

    if (empty($errors)) {
        $users = json_decode(file_get_contents("users.json"), true);

        foreach ($users as $user) {
            if ($user["email"] === $email) {
                $errors[] = "Ezzel az e-mail címmel már létezik felhasználó.";
                break;
            }
        }

        if (empty($errors)) {
            $users[] = [
                "name" => $name,
                "email" => $email,
                "password" => password_hash($password, PASSWORD_DEFAULT),
                "admin" => false
            ];
            file_put_contents("users.json", json_encode($users, JSON_PRETTY_PRINT));
            $_SESSION["user_id"] = count($users) - 1;
            header("location: index.php");
            exit();
        }
    }
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>Regisztráció</title>
</head>
<body>
    <ul>
        <li><a>iKarRental</a></li>
        <li class="link"><a href="login.php">Bejelentkezés</a></li>
        <li class="link"><a href="index.php">Főoldal</a></li>
    </ul>
    <form method="post" action="register.php" >
        Teljes név:<input type="text" name="name"><br>
        <?= $errors['name'] ?? '' ?><br>
        E-mail cím:<input type="text" name="email"><br>
        <?= $errors['email'] ?? '' ?><br>
        Jelszó:<input type="password" name="password"><br>
        <?= $errors['password'] ?? '' ?><br>
        Jelszó még egyszer:<input type="password" name="confirm_password"><br>
        <?= $errors['confirm'] ?? '' ?><br>
        <button type="submit">Regisztráció</button>
    </form>
</body>
</html>