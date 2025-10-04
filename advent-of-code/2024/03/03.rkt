#lang racket

(define PROD #t)

;(define input (make-parameter "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))"))
(define input (make-parameter "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"))
(when PROD (input (file->string "03.txt")))


(define (parse-instructions s)
   (regexp-match* #px"mul\\(\\d+,\\d+\\)|do\\(\\)|don't\\(\\)" s))


(define (interpret instructions [include-dos #f])
  (for/fold ([enabled #t]
             [acc 0]
             #:result acc)
            ([instruction instructions])
    (match instruction
      [(pregexp #px"don't\\(\\)") #:when (and include-dos enabled)
                        (values #f acc)]
      [(pregexp #px"do\\(\\)") #:when (and include-dos (not enabled))
                     (values #t acc)]
      [(pregexp #px"mul\\(\\d+,\\d+\\)") #:when enabled
                               (let* ([digit-strings (regexp-match* #px"\\d+" instruction)]
                                      [digits (map string->number digit-strings)]
                                      [sum (apply * digits)])
                                 (values enabled (+ acc sum)))]
      [_ (values enabled acc)])))

; Part 1 ----------------------------------------------------

(interpret (parse-instructions (input)))

; Part 2 ----------------------------------------------------
(interpret (parse-instructions (input)) #t)

