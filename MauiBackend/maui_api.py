from flask import Flask, jsonify, request
from flask_restful import Api, Resource
from flask_cors import CORS
import sqlite3

app = Flask(__name__)
CORS(app)
api = Api(app)

class CheckApkVersion(Resource):
    def get(self):

        conn = sqlite3.connect('/d:/SourceCode/SideProject/MAUI_Project/MauiBackend/maui.db')
        cursor = conn.cursor()

        apk_parameter = request.args.get('apk')
        print("apk_parameter : " , request.args.get('apk'))
        
        cursor.execute("SELECT * FROM apkversion WHERE apk = ?", (apk_parameter,))
        result = cursor.fetchone()
        print("result : " , result)

        conn.close()

        if result:
            return jsonify({"apk": result[1], "version": result[2] })
        else:
            return jsonify({"message": "APK not found"}), 404


class Login(Resource):
    def post(self):
        data = request.get_json()
        username = data.get('username')
        password = data.get('password')
        # Implement your authentication logic here
        if username == "admin" and password == "password":
            return jsonify({"message": "Login successful", "token": "fake-jwt-token"})
        else:
            return jsonify({"message": "Invalid credentials"}), 401

api.add_resource(CheckApkVersion, '/api/Common/CheckApkVersion')
api.add_resource(Login, '/api/Common/Login')

if __name__ == '__main__':
    app.run(debug=True)