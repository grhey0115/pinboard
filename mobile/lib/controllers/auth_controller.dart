import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class AuthController {
  final String baseUrl = "http://localhost:5062/api/User"; // The correct API base URL

  Future<bool> signIn(String username, String password) async {
    final url = Uri.parse('$baseUrl/authenticate');
    final response = await http.post(
      url,
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({
        "username": username, // Sending username and password in the request body
        "password": password
      }),
    );

    if (response.statusCode == 200) {
      // Assuming the response contains a token, you should parse it here
      final responseBody = jsonDecode(response.body);
      final token = responseBody['token']; // If your API returns a token
      // Save the token, handle authentication logic...
      
      return true;
    } else {
      return false;
    }
  }
}
