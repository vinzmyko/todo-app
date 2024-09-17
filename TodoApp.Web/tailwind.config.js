/** @type {import('tailwindcss').Config} */

module.exports = {
  content: [
    './Pages/**/*.{razor,html,cshtml}',
    './Components/**/*.{razor,html,cshtml}',
    './wwwroot/**/*.html',
  ],
  theme: {
    extend: {
      fontFamily: {
        primary: ['Oswald', 'sans-serif'],
        secondary: ['Lato', 'sans-serif'],
        accent: ['Raleway', 'sans-serif'],
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}