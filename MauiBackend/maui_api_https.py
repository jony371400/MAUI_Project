from flask import Flask, jsonify, request
from flask_restful import Api, Resource
from flask_cors import CORS
import sqlite3

app = Flask(__name__)
CORS(app)
api = Api(app)

def get_db_connection():
    conn = sqlite3.connect('/d:/SourceCode/SideProject/MAUI_Project/MauiBackend/maui.db')
    conn.row_factory = sqlite3.Row
    return conn

class CheckApkVersion(Resource):
    def get(self):
        apk_parameter = request.args.get('apk')

        if not apk_parameter:
            return jsonify({"message": "apk parameter is required"}), 400

        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM apkversion WHERE apk = ?", (apk_parameter,))
        result = cursor.fetchone()
        conn.close()

        if result:
            return jsonify({"apk": result["apk"], "version": result["version"]})
        else:
            return jsonify({"message": "APK not found"}), 404

class Login(Resource):
    def post(self):
        data = request.get_json()
        username = data.get('username')
        password = data.get('password')

        if not username or not password:
            return jsonify({"message": "Username and password are required"}), 400

        conn = get_db_connection()
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM users WHERE username = ? AND password = ?", (username, password))
        user = cursor.fetchone()
        conn.close()

        if user:
            return jsonify({"message": "Login successful", "token": "fake-jwt-token"})
        else:
            return jsonify({"message": "Invalid credentials"}), 401

api.add_resource(CheckApkVersion, '/api/Common/CheckApkVersion')
api.add_resource(Login, '/api/Common/Login')

if __name__ == '__main__':
    # 啟動 HTTPS
    # app.run(ssl_context=('cert.pem', 'key.pem'), debug=True)
    app.run(host='0.0.0.0', port=5000, ssl_context=('cert.pem', 'key.pem'), debug=True)
