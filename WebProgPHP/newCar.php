<?php
session_start();
$users = json_decode(file_get_contents("users.json"), true);
$user = isset($_SESSION["user_id"]) ? $users[$_SESSION["user_id"]] : null;

?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>Új autó hozzáadása</title>
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
<h1>Új autó hozzáadása</h1>
    <form method="POST" action="" enctype="multipart/form-data">
        <label for="brand">Márka:</label>
        <input type="text" id="brand" name="brand" required><br><br>

        <label for="model">Modell:</label>
        <input type="text" id="model" name="model" required><br><br>

        <label for="transmission">Váltó típusa:</label>
        <select id="transmission" name="transmission" required>
            <option value="">Válassz...</option>
            <option value="Automata">Automatic</option>
            <option value="Manuális">Manual</option>
        </select><br><br>

        <label for="passengers">Férőhelyek száma:</label>
        <input type="number" id="passengers" name="passengers" min="1" required><br><br>

        <label for="daily_price_huf">Napidíj (HUF):</label>
        <input type="number" id="daily_price_huf" name="daily_price_huf" min="1" required><br><br>

        <label for="year">Évjárat:</label>
        <input type="number" id="year" name="year" min="1900" max="<?= date('Y') ?>" required><br><br>

        <label for="fuel_type">Üzemanyag típusa:</label>
        <input type="text" id="fuel_type" name="fuel_type" required><br><br>

        <label for="image">Kép feltöltése:</label>
        <input type="file" id="image" name="image" accept="image/*" required><br><br>

        <button type="submit" name="submit">Mentés</button>
    </form>
    <?php
    if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['submit'])) {
    $json_file = 'cars.json';
    $cars = file_exists($json_file) ? json_decode(file_get_contents($json_file), true) : [];

    $errors = [];

    $brand = trim($_POST['brand']);
    $model = trim($_POST['model']);
    $transmission = $_POST['transmission'];
    $passengers = (int)$_POST['passengers'];
    $daily_price_huf = (int)$_POST['daily_price_huf'];
    $year = (int)$_POST['year'];
    $fuel_type = trim($_POST['fuel_type']);
    $image = $_FILES['image'];

    if (empty($brand)) $errors[] = "A márka megadása kötelező.";
    if (empty($model)) $errors[] = "A modell megadása kötelező.";
    if (empty($transmission)) $errors[] = "A váltó típusának kiválasztása kötelező.";
    if ($passengers < 1) $errors[] = "A férőhelyek száma legalább 1 kell, hogy legyen.";
    if ($daily_price_huf < 1) $errors[] = "A napidíjnak pozitív számnak kell lennie.";
    if ($year < 1900 || $year > date('Y')) $errors[] = "Az évjárat 1900 és " . date('Y') . " között kell legyen.";
    if (!($fuel_type === "Petrol") || !($fuel_type === "Diesel") || !($fuel_type === "Electric")) $errors[] = "Az üzemanyag lehetséges típusai: Petrol, Diesel, Electric!";
    if (empty($fuel_type)) $errors[] = "Az üzemanyag típusának megadása kötelező.";

    if ($image['error'] === UPLOAD_ERR_OK) {
        $allowed_extensions = ['jpg', 'jpeg', 'png', 'gif'];
        $file_extension = pathinfo($image['name'], PATHINFO_EXTENSION);

        if (!in_array(strtolower($file_extension), $allowed_extensions)) {
            $errors[] = "A kép csak .jpg, .jpeg, .png vagy .gif formátumban lehet.";
        }
    } else {
        $errors[] = "A kép feltöltése sikertelen.";
    }

    if (empty($errors)) {
        $upload_dir = 'uploads/';
        if (!is_dir($upload_dir)) {
            mkdir($upload_dir, 0777, true);
        }

        $image_path = $upload_dir . uniqid() . '.' . $file_extension;
        move_uploaded_file($image['tmp_name'], $image_path);

        $new_car = [
            'id' => count($cars) + 1,
            'brand' => $brand,
            'model' => $model,
            'transmission' => $transmission,
            'passengers' => $passengers,
            'daily_price_huf' => $daily_price_huf,
            'year' => $year,
            'fuel_type' => $fuel_type,
            'image' => $image_path
        ];

        $cars[] = $new_car;

        file_put_contents($json_file, json_encode($cars, JSON_PRETTY_PRINT));

        echo "<p style='color:green;'>Az autó sikeresen hozzáadva!</p>";
    } else {
        foreach ($errors as $error) {
            echo "<p style='color:red;'>$error</p>";
        }
    }
    }
    ?>
</body>
</html>