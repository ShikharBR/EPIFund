import React, { Component } from "react"
import ReactDOM from 'react-dom'
import partners from 'APP/pages/nav/partners'

class FooterMain extends Component {
  constructor(props) {
    super(props);

    this.state = {

    }
  }

  render() {
    return (
      <React.Fragment>
        <div className="footertopwrap">
          <div className="smallfooterwrap logoiconsfooter">
            <img alt="" src="https://s3-us-west-1.amazonaws.com/dev-uscreonline-content/uscrelogo_white.png" style={{ cursor: "pointer", maxWidth: "250px" }} onClick={() => { location.href = "/Home/" }} />
            <div className="footer_tinyicons" style={{ paddingTop: "0px", marginTop: "-15px", marginLeft: "17px" }}>
              <a href="#"><img alt="" src="https://i.imgur.com/ohgEBlH.png" style={{ maxWidth: "34px" }} /></a>
              <a href="#"><img alt="" src="https://i.imgur.com/8H4kW67.png" className="tinyiconpad" style={{ maxWidth: "34px" }} /></a>
              <a href="#"><img alt="" src="https://i.imgur.com/axnHUJm.png" className="tinyiconpad" style={{ maxWidth: "34px" }} /></a>
            </div>
          </div>
          <div className="bigfooterwrap">
            <Carousel />
          </div>
          <div className="smallfooterwrap flexdisplay">
            <div className="smallfooter_halfwrap">
              <h3>Resources</h3>
              <a href="/Home/">Home</a>
              <a href="/Home/MyUSCPage">My USCRE</a>
              <a href="/Home/ContactUs">Contact Us</a>
              <a href="/Home/Affiliations">Partners</a>
            </div>

            <div className="smallfooter_halfwrap">
              <h3>Company</h3>
              <a href="/Home/AboutUs">About Us</a>
              <a href="/Home/EmploymentOpportunities">Opportunities</a>
              <a href="/Home/PrivacyPolicy">Privacy Policy</a>
            </div>
          </div>
        </div>
        <div className="footerbottomwrap">
          <h2>© 2019 USCRE ONLINE INC - ALL RIGHTS RESERVED</h2>
          <div className="alignrighttext">USCRE is Wholly Owned by USC Global Holdings, Inc.<br />USCRE is Designed and Maintained by Decision Management Technologies, LLC</div>
        </div>
      </React.Fragment>
    );
  }
}

// const imgUrls = [
// ];

// for (var i = 1; i < 59; i++) {
//     imgUrls.push("http://d34jlwp7y45ovp.cloudfront.net/footericons/" + i + ".png");
// }


// console.log(imgUrls);

class Carousel extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      currentImageIndex: 0,
      intervalId: null,
    };

    this.nextSlide = this.nextSlide.bind(this);
    this.previousSlide = this.previousSlide.bind(this);
    this.nextBig = this.nextBig.bind(this);
    this.mouseOver = this.mouseOver.bind(this);
    this.mouseLeave = this.mouseLeave.bind(this);
  }

  previousSlide() {
    const lastIndex = partners.length - 1;
    const { currentImageIndex } = this.state;
    const shouldResetIndex = currentImageIndex === 0;
    const index = shouldResetIndex ? lastIndex : currentImageIndex - 1;

    this.setState({
      currentImageIndex: index
    });
  }

  nextSlide() {
    const lastIndex = partners.length - 1;
    const { currentImageIndex } = this.state;
    const shouldResetIndex = currentImageIndex === lastIndex;
    const index = shouldResetIndex ? 0 : currentImageIndex + 1;

    this.setState({
      currentImageIndex: index
    });
  }

  nextBig() {
    const lastIndex = partners.length - 1;
    const { currentImageIndex } = this.state;
    const shouldResetIndex = currentImageIndex === lastIndex;
    const indexthree = currentImageIndex + 1;
    let index;
    if (!partners[indexthree]) {
      index = 0;
    }
    else {
      index = shouldResetIndex ? 0 : currentImageIndex + 1;
    }

    this.setState({
      currentImageIndex: index
    });
  }

  componentDidMount() {
    var interval = setInterval(this.nextBig, 1500);
    this.setState({ intervalId: interval });
  }

  mouseOver() {
    clearInterval(this.state.intervalId);
  }

  mouseLeave() {
    var interval = setInterval(this.nextBig, 1500);
    this.setState({ currentImageIndex: this.state.currentImageIndex + 1, intervalId: interval });
  }

  render() {
    let imgarr = [];
    for (var i = 0; i < 4; i++) {
      if (partners[this.state.currentImageIndex + i]) {
        imgarr.push(partners[this.state.currentImageIndex + i]);
      }
    }
    return (
      <React.Fragment>
        <div className="carousel" onMouseEnter={this.mouseOver} onMouseLeave={this.mouseLeave}>
          <h5>USCRE is proud to be a member of these trusted industry organizations</h5>
          <Arrow direction="left" clickFunction={this.previousSlide} glyph="&#9664;" />
          {
            imgarr.map(item => {
              if (item.imglink && item.sitelink) {
                return <ImageSlide key={item.key} url={item.imglink} link={item.sitelink} />
              }
            })
          }
          <Arrow direction="right" clickFunction={this.nextSlide} glyph="&#9654;" />
        </div>
      </React.Fragment>
    );
  }
}

const Arrow = ({ direction, clickFunction, glyph }) => (
  <div
    className={`slide-arrow ${direction}`}
    onClick={clickFunction}>
    {glyph}
  </div>
);

class ImageSlide extends React.Component {
  constructor(props) {
    super(props);

    this.state = {

    }

    this.goPartner = this.goPartner.bind(this);
  }

  goPartner(url) {
    window.open(url, '_blank');
  }

  render() {
    const styles = {
      backgroundImage: `url(${this.props.url})`,
      backgroundSize: 'contain',
      backgroundRepeat: 'no-repeat',
      backgroundPosition: 'center',
      marginLeft: "4px",
      marginRight: "4px",
      height: '70%'
    };
    return (
      <div className="image-slide animated slideInRight faster" style={styles} onClick={() => this.goPartner(this.props.link)} key={Math.random()}>
      </div>
    );
  }
}


ReactDOM.render(<FooterMain />, document.getElementById('footermain'));