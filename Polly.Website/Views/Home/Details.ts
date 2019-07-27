namespace Polly {

    export class Details {

        constructor() {
            $(".js_readmore").readmore({
                speed: 200,
                collapsedHeight: 200,
                moreLink: '<a href="#">Read more</a>',
                lessLink: '<a href="#">Read less</a>',
                afterToggle: function (trigger:any, element:Element, expanded:boolean) {
                    if (!expanded) { // The "Close" link was clicked
                        $('html, body').animate({ scrollTop: $(element).offset().top }, { duration: 100 });
                    }
                }
            });
        }
    }
}