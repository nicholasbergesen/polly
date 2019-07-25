namespace Polly {

    export class Details {

        constructor() {
            $(".js_readmore").readmore({
                collapsedHeight: 200,
                moreLink: '<a href="#">Read more</a>',
                lessLink: '<a href="#">Read less</a>',
                blockCSS: 'overflow-y:hidden;',
            });
        }
    }
}