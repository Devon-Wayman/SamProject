<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<br />
<div align="center">
  <h3 align="center">SAM Project</h3>

  <p align="center">
   Official project documentation for SAM (Smoke and Mirrors)
  </p>
</div>


## About The Project

* Viewer perspective example:
![Product Name Screen Shot][viewer-image]

* Performer setup example:
![operation-image]


### Built With

The following programs and libraries were used within this application's development:
* [Riptide][riptide-url]
* [Unity Engine][unity-url]
* [Visual Studio][vs-url]

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites
Hardware Needed: 

* iDevice (iPhone or iPad with face tracking capability)
* OSX laptop or desktop (Windows platform support currently in the works)
* A wireless router (5G band capable recommended). An adaptor to directly connect the iDevice and computer hosting the server can also be used
* Soft LED light - Recommended to evenly illumiate performer's face to aid in facial tracking
* XCode - Required to build the mobile app used for tracking performers face and sending data to the server

### Installation

The instructions to setup the application are as follows 

1. Download the server build on an OSX machine (Windows support coming soon) as well as the XCode source project
You will need an Apple developer account to build and deploy the mobile app required for face tracking at this time though more universal methods are being looked into.

2. Launch the server application on the computer you intend to project from. You will be prompted to allow incoming connections; allow these. Failure to do so will prevent the mobile app on 
your LAN from reaching the server to update the face model.

3. Build and deploy the mobile app to an iDevice via XCode. iOS 15 or greater is required

4. Launch the mobile app. You will be asked to allow connections to devices on your local network. Again, allow this so that the mobile app can
send the required data to the server. 

5. With the mobile and server app open, enter the iPv4 address of the server machine assigned by your router. If the address is correct and proper networking permissions have been granted, you
should see the head model appear within the server application and follow the user's motions and expressions. 


## Usage and Tips
* The application can be simply projected onto a wall though using polyester chiffon  (recommend solid-platinum) is highly recommended as it is a great material to use for projecting images onto. With the proper lighting the fabric itself 
will be practically invisible, making the head appear to be floating in the air.

* It is recommended that the performer have a soft LED light illuminating their face during use.
While the main facial expression system is powered via the IR depth sensor of the iDevice, adaquet lighting
will increase tracking accuracy and reduce stutter

* To lower latency and decrease throttling on the user's local network, a LAN to Lightning Bolt/USB-C connector can be used
to directly connect the mobile device and the laptop hosting the server directly. This will decrease traffic on the local wifi
and greatly improve performance


## Contact
Devon Wayman -  devonwayman97@gmail.com

Project Link: [Here](https://github.com/Devon-Wayman/SamProject)


## Acknowledgments
Special thanks to the following developers for their resources, inspiration and guidance

* [Dilmer Valecillos](https://www.youtube.com/@dilmerv) - For his treasure trove of AR development based educational resources.
* [Tom Weiland](https://www.youtube.com/@tomweiland) - For his open source networking framework, Riptide. 


<!-- MARKDOWN LINKS & IMAGES -->
[contributors-shield]: https://img.shields.io/github/contributors/Devon-Wayman/SamProject.svg?style=for-the-badge
[contributors-url]: https://github.com/Devon-Wayman/SamProject/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/Devon-Wayman/SamProject.svg?style=for-the-badge
[forks-url]: https://github.com/Devon-Wayman/SamProject/network/members
[stars-shield]: https://img.shields.io/github/stars/Devon-Wayman/SamProject.svg?style=for-the-badge
[stars-url]: https://github.com/Devon-Wayman/SamProject/stargazers
[issues-shield]: https://img.shields.io/github/issues/Devon-Wayman/SamProject.svg?style=for-the-badge
[issues-url]: https://github.com/Devon-Wayman/SamProject/issues
[license-shield]: https://img.shields.io/github/license/Devon-Wayman/SamProject.svg?style=for-the-badge
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/devon-wayman/
[viewer-image]: images/viewer.gif
[operation-image]: images/operation.gif
[riptide-url]: https://github.com/RiptideNetworking/Riptide
[unity-url]: https://unity.com/
[vs-url]: https://visualstudio.microsoft.com
