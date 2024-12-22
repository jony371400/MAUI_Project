from flask import Flask, jsonify, request
from flask_restful import Api, Resource
from flask_cors import CORS
import sqlite3

app = Flask(__name__)
CORS(app)
api = Api(app)

def get_db_connection():
    conn = sqlite3.connect('/d:/SourceCode/SideProject/Project-FullStack-MauiApp/MauiBackend/maui.db')
    conn.row_factory = sqlite3.Row
    return conn

class CheckApkVersion(Resource):
    def get(self):
        print("CheckApkVersion API Start")
        apk_parameter = request.args.get('apk')

        if not apk_parameter:
            return jsonify({"status": "0009", "message": "APK parameter is required"})

        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM apkversion WHERE apk = ?", (apk_parameter,))
        result = cursor.fetchone()
        conn.close()

        if result:
            return jsonify({"status": "0000", "message": "APK found", "apk": result["apk"], "version": result["version"]})
        else:
            return jsonify({"status": "0001", "message": "APK not found"})

class CheckUserInfomation(Resource):
    def post(self):
        print("CheckUserInfomation API Start")
        data = request.get_json()
        username = data.get('username')
        password = data.get('password')

        if not username or not password:
            return jsonify({"status": "0009", "message": "Username and password are required"})

        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM users WHERE username = ? AND password = ?", (username, password))
        user = cursor.fetchone()
        conn.close()

        if user:
            return jsonify({"status": "0000", "message": "Login successful"})
        else:
            return jsonify({"status": "0001", "message": "Invalid credentials"})

class CreateUser(Resource):
    def post(self):
        print("CreateUser API Start")
        data = request.get_json()
        username = data.get('username')
        password = data.get('password')

        if not username or not password:
            return jsonify({"status": "0009", "message": "Username and password are required"})

        conn = get_db_connection()
        cursor = conn.cursor()

        cursor.execute("SELECT * FROM users WHERE username = ?", (username,))
        existing_user = cursor.fetchone()
        if existing_user:
            conn.close()
            return jsonify({"status": "0001", "message": "Username already exists"})

        cursor.execute("INSERT INTO users (username, password) VALUES (?, ?)", (username, password))
        conn.commit()
        conn.close()

        return jsonify({"status": "0000", "message": "User created successfully"})

api.add_resource(CheckApkVersion, '/api/Common/CheckApkVersion')
api.add_resource(CheckUserInfomation, '/api/Common/CheckUserInfomation')
api.add_resource(CreateUser, '/api/Common/CreateUser')

if __name__ == '__main__':
    # #Start HTTPS
    # app.run(ssl_context=('cert.pem', 'key.pem'), debug=True) # Default
    # app.run(host='10.249.77.16', port=5000, ssl_context=('cert.pem', 'key.pem'), debug=True) # Company
    app.run(host='192.168.22.53', port=5000, ssl_context=('cert.pem', 'key.pem'), debug=True) # Home
