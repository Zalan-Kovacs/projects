<?php
session_start();
$users = json_decode(file_get_contents("users.json"), true);
$user = isset($_SESSION["user_id"]) ? $users[$_SESSION["user_id"]] : null;

$cars = json_decode(file_get_contents('cars.json'), true);

$carId = isset($_GET['id']) ? (int)$_GET['id'] : 0;



// Az autó adatai megjelenítése

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>Document</title>
</head>
<body>
    <ul>
        <li><a>iKarRental</a></li>
        <?php if ($user): ?>
            <li><a>Bejelentkezve: <?= $user["name"] ?></a></li>
            <li class="link"><a href="logout.php">Kijelentkezés</a></li>
            <li class="link"><a href="index.php">Főoldal</a></li>
        <?php else: ?>
            <li class="link"><a href="register.php">Regisztráció</a></li>
            <li class="link"><a href="login.php">Bejelentkezés</a></li>
            <li class="link"><a href="index.php">Főoldal</a></li>
        <?php endif; ?>
    </ul>
    <?php
        $selectedCar = null;
        foreach ($cars as $car) {
            if ($car['id'] === $carId) {
                $selectedCar = $car;
                break;
            }
        }

        if (!$selectedCar) {
            echo '<p>Hiba: Az autó nem található!</p>';
            echo '<a href="index.php">Vissza a főoldalra</a>';
            exit;
        }
        echo '<div class="car-details">';
        echo '<img src="' . $selectedCar['image'] . '" alt="' . $selectedCar['brand'] . ' ' . $selectedCar['model'] . '">';
        echo '<h1>' . $selectedCar['brand'] . ' ' . $selectedCar['model'] . '</h1>';
        echo '<p>Gyártási év: ' . $selectedCar['year'] . '</p>';
        echo '<p>Váltó: ' . $selectedCar['transmission'] . '</p>';
        echo '<p>Üzemanyag: ' . $selectedCar['fuel_type'] . '</p>';
        echo '<p>Férőhelyek száma: ' . $selectedCar['passengers'] . '</p>';
        echo '<p>Napidíj: ' . number_format($selectedCar['daily_price_huf'], 0, ',', ' ') . ' Ft</p>';
        echo '<a href="index.php"><button>Vissza</button></a>';
        echo '</div>';
    ?>
</body>
</html>