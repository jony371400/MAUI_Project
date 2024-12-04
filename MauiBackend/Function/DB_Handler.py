import sqlite3

# Initialize SQLite database
def init_db():
    conn = sqlite3.connect('maui.db')
    cursor = conn.cursor()
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username TEXT NOT NULL UNIQUE,
            password TEXT NOT NULL
        )
    ''')
    conn.commit()

    cursor.execute('''
        CREATE TABLE IF NOT EXISTS apkversion (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            apk TEXT NOT NULL UNIQUE,
            version TEXT NOT NULL
        )
    ''')
    conn.commit()

    conn.close()

# Create a user with the given username and password
def create_user(username, password):
    conn = sqlite3.connect('maui.db')
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO users (username, password) VALUES (?, ?)
    ''', (username, password))
    conn.commit()
    conn.close()

# Create a user with the given username and password
def create_apkversion(apk, version):
    conn = sqlite3.connect('maui.db')
    cursor = conn.cursor()
    cursor.execute('''
        INSERT INTO apkversion (apk, version) VALUES (?, ?)
    ''', (apk, version))
    conn.commit()
    conn.close()

# init_db()
# create_user('11107647', '000000')
# create_apkversion('maui', '1.0')


