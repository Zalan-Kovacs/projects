<?php
session_start();
$users = json_decode(file_get_contents("users.json"), true);
$user = isset($_SESSION["user_id"]) ? $users[$_SESSION["user_id"]] : null;
$admin = isset($_SESSION["user_id"]) ? $user['admin'] : false;
$json_data = file_get_contents('cars.json');
$cars = json_decode($json_data, true);

$transmission = $_POST['transmission'] ?? '';
$passengers = $_POST['passengers'] ?? '';
$min_price = $_POST['min_price'] ?? '';
$max_price = $_POST['max_price'] ?? '';

$validCars = [];
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>iKarRental</title>
</head>
<body>
    <ul>
        <li><a>iKarRental</a></li>
        <?php if ($admin): ?>
            <li class="link"><a href="newCar.php">Új autó</a></li>
        <?php endif; ?>
        <?php if ($user): ?>
            <li><a>Bejelentkezve: <?= $user["name"] ?></a></li>
            <li class="link"><a href="logout.php">Kijelentkezés</a></li>
        <?php else: ?>
            <li class="link"><a href="register.php">Regisztráció</a></li>
            <li class="link"><a href="login.php">Bejelentkezés</a></li>
        <?php endif; ?>
    </ul>
        
    <div>
        <h1>Kölcsönözz autókat könnyedén!</h1>
        <div class="container">
        <form action="index.php"method="POST">
            <div>
                <label for="start_date">Kezdő dátum</label>
                <input type="number" id="start_date" name="start_date">
            </div>
            <div>
                <label for="end_date">Befejező dátum</label>
                <input type="number" id="end_date" name="end_date">
            </div>
            <div>
                <label for="transmission">Váltó típusa</label>
                <select id="transmission" name="transmission" >
                    <option value="" <?= $transmission == "" ? 'selected' : ''?>>Mind</option>
                    <option value="Manual" <?= $transmission == "Manual" ? 'selected' : ''?>>Manual</option>
                    <option value="Automatic" <?= $transmission == "Automatic" ? 'selected' : ''?>>Automatic</option>
                </select>
            </div>
            <div>
                <label for="passengers">Férőhelyek száma (minimum)</label>
                <input type="number" id="passengers" name="passengers" min="1" value="<?= $passengers ?>">
            </div>
            <div>
                <label for="min_price">Napidíj minimum</label>
                <input type="number" id="min_price" name="min_price" min="0" value="<?= $min_price ?>">
            </div>
            <div>
                <label for="max_price">Napidíj maximum</label>
                <input type="number" id="max_price" name="max_price" min="0" value="<?= $max_price ?>">
            </div>
            <div>
                <button type="submit">Szűrés</button>
            </div>
        </form>
        <form method="POST" action="index.php">
                <button type="submit">Szűrés törlése</button>
            </form>
    </div>
    <div class="container">
    <div class="cars">
            <?php
            
            
            
            
            foreach ($cars as $car) {
                $isValidCar = true;
                if ($transmission && $car['transmission'] !== $transmission) {
                    $isValidCar = false;
                }
                if ($passengers && $car['passengers'] < $passengers) {
                    $isValidCar = false;
                }
                if ($min_price && $max_price && ($car['daily_price_huf'] < $min_price || $car['daily_price_huf'] > $max_price)) {
                    $isValidCar = false;
                }
                if($isValidCar){
                    $validCars[] = $car;
                }
            }
            
            if(count($validCars) > 0){
                foreach ($validCars as $car) {
                echo '<div class="car-card">';
                echo '<a href="car.php?id=' . $car['id'] . '"><img src="' . $car['image'] . '" alt="' . $car['brand'] . ' ' . $car['model'] . '"></a>';
                echo '<h2>' . $car['brand'] . ' ' . $car['model'] . '</h2>';
                echo '<p>' . $car['year'] . ' | ' . $car['transmission'] . ' | ' . $car['fuel_type'] . '</p>';
                echo '<p>' . $car['passengers'] . ' passenger</p>';
                echo '<p class="price">' . number_format($car['daily_price_huf'], 0, ',', ' ') . ' Ft / nap</p>';
                echo '<button>Foglalás</button>';
                echo '</div>';
                }
            }else{
                echo '<p>Nincsenek elérhető autók a megadott szűrési feltételekkel.</p>';
            }

            ?>
        </div>
    </div>
        </div>
</body>
</html>