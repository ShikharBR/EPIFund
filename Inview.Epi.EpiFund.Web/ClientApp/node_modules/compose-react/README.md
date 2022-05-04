# compose-react

A higher order function for wrapping react components many times.

## Install

```bash
npm i -S compose-react
```

## Why do i need this?
If your project uses HOC's (Higher-Order Components) like react-redux/react-router to wrap react components

## Motivation
Using more than 1 HOC results in deeply nested or verbose code
```js
// deeply nested single line statement
export default withStyles(styles)(withRouter(connect(mapDispatchToProps, mapDispatchToProps)(MyReactButton)));

// verbose multiple assignments
MyReactButton = withStyles(styles)(MyReactButton);
MyReactButton = withRouter(MyReactButton);
MyReactButton = connect(mapDispatchToProps, mapDispatchToProps)(MyReactButton);
export default MyReactButton;
```
## Composition
`compose-react` will allow HOC's to be composed
```js
export default compose(
  withStyles(styles),
  withRouter,
  connect(mapStateToProps, mapDispatchToProps)
)(MyReactButton);
```

## Usage
How to use `compose-react`'s compose method

```js
import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import { connect } from 'react-redux';
import { compose } from 'compose-react';

const styles = theme => ({
  container: {
    backgroundColor: theme.palette.primary.main,
    padding: theme.spacing.unit
  }
});

const MyReactButton = ({ isFetching, handleClick, classes }) => (
  <div className={classes.container}>
    <input
      onClick={handleClick}
      disabled={isFetching}
      type="button"
      value="Click me!"
    />
  </div>
);

const mapStateToProps = state => ({
  isFetching: state.isFetching
});
const mapDispatchToProps = dispatch => ({
  handleClick: () => dispatch({ type: 'CLICKED' })
});

export default compose(
  withStyles(styles),
  withRouter,
  connect(
    mapStateToProps,
    mapDispatchToProps
  )
)(MyReactButton);
```

## License

[MIT](http://vjpr.mit-license.org)
